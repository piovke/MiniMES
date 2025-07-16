using Microsoft.AspNetCore.Mvc;
using MiniMES.Models;
using Microsoft.EntityFrameworkCore;
using MiniMES.DTOs;
namespace MiniMES.Controllers

{
    [Route("Processes")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        private readonly MiniProductionDbContext _context;

        public ProcessController(MiniProductionDbContext context)
        {
            _context = context;
        }
        
        [HttpPost]
        [Route("AddProcess")]
        public IActionResult AddProcess([FromBody] CreateProcessDto input)
        {
           Process process = new Process
            {
                SerialNumber = input.SerialNumber,
                OrderId = input.OrderId,
                Status = input.Status,
                DateTime = DateTime.Now
            };
            
            _context.Processes.Add(process);
            _context.SaveChanges();
            return Ok($"added \"{process.SerialNumber}\"");
        }

        [HttpGet]
        [Route("ShowProcesses")]
        public IActionResult ShowProcesses()
        {
            if (!_context.Processes.Any())
            {
                return NoContent();
            }
            
            var processesDto = _context.Processes
                .Include(p => p.ProcessParameters)
                .Include(p=>p.Order)
                .Select(p=> new ProcessDto
                {
                    Id = p.Id,
                    SerialNumber = p.SerialNumber,
                    Order = new OrderDto
                    {
                        Id = p.Order.Id,
                        Code = p.Order.Code,
                    },
                    Status = p.Status,
                    DateTime = p.DateTime
                }).ToList();
            return Ok(processesDto);
        }

        [HttpDelete]
        [Route("DeleteProcess")]
        public IActionResult DeleteProcess([FromQuery] int id)
        {
            if (!_context.Processes.Any(p => p.Id == id))
            {
                return BadRequest("No process with this id");
            }
            _context.Processes.Remove(_context.Processes.First(p => p.Id == id));
            _context.SaveChanges();
            return Ok($"deleted \"{id}\"");
        }

        [HttpPut]
        [Route("UpdateProcess")]
        public IActionResult UpdateProcess([FromQuery] int id, [FromBody] CreateProcessDto input)
        {
            Process? processToUpdate = _context.Processes.Find(id);

            if (processToUpdate == null)
            {
                return BadRequest("No process with this id");
            }
            
            processToUpdate.SerialNumber = input.SerialNumber;
            processToUpdate.OrderId = input.OrderId;
            processToUpdate.Status = input.Status;
            _context.Processes.Update(processToUpdate);
            _context.SaveChanges();
            return Ok($"updated \"{id}\"");
        }

        [HttpGet]
        [Route("Details/{id}")]
        public IActionResult Details([FromRoute] int id)
        {
            var process = _context.Processes
                .Include(p=>p.Order)
                .Include(p=>p.ProcessParameters)
                .ThenInclude(o=>o.Parameter)
                .FirstOrDefault(p => p.Id == id);
            if (process == null)
            {
                return NotFound();
            }

            var processDto = new ProcessDto
            {
                Id = process.Id,
                SerialNumber = process.SerialNumber,
                Status = process.Status,
                DateTime = process.DateTime,
                Order = new OrderDto
                {
                    Id = process.Order.Id,
                    Code = process.Order.Code,
                },
                ProcessParameters = process.ProcessParameters.Select(p => new ProcessParameterDto
                {
                    Id = p.Id,
                    Value = p.Value,
                    Parameter = new ParameterDto
                    {
                        Id = p.Parameter.Id,
                        Name = p.Parameter.Name,
                        Unit = p.Parameter.Unit,
                    }
                }).ToList()
            };

            return Ok(processDto);

        }
    }
}