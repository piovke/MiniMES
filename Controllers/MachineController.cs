using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniMES.Models;
using MiniMES.DTOs;

namespace MiniMES.Controllers

{
    [Route("Machines")]
    [ApiController]
    public class MachineController : ControllerBase
    {
        private readonly MiniProductionDbContext _context;
        public MachineController(MiniProductionDbContext context)
        {
            _context = context;
        }
        
        [HttpPost]
        [Route("AddMachine")]
        public IActionResult AddMachine([FromBody] CreateMachineDto input)
        {
            Machine machine = new Machine
            {
                Name = input.Name,
                Description = input.Description
            };
            
            _context.Machines.Add(machine);
            _context.SaveChanges();
            return Ok($"added \"{machine.Name}\"");
        }

        [HttpGet]
        [Route("ShowMachines")]
        public IActionResult ShowMachines()
        {
            if (!_context.Machines.Any())
            {
                Console.WriteLine("No machines to show");
                return NoContent();
            }
            
            var machines = _context.Machines
                .Include(m=>m.Orders)
                .Select(m => new MachineDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Orders = m.Orders.Select(o=>new OrderDto
                    {
                        OrderId = o.Id,
                        Code = o.Code,
                    }).ToList(),
                }).ToList();
            
            return Ok(machines);
        }
        
        [HttpGet]
        [Route("Details/{id}")]
        public IActionResult Details([FromRoute] int id)
        {
            var machine = _context.Machines
                .Include(m => m.Orders)
                .FirstOrDefault(m => m.Id == id);

            if (machine == null)
            {
                return NotFound();
            }

            var machineDto = new MachineDto
            {
                Id = machine.Id,
                Name = machine.Name,
                Description = machine.Description,
                Orders = machine.Orders.Select(o=>new OrderDto
                {
                    OrderId = o.Id,
                    Code = o.Code,
                }).ToList()
            };

            return Ok(machineDto);
        }


        [HttpDelete]
        [Route("DeleteMachine")]
        public IActionResult DeleteMachine([FromQuery] int id)
        {
            var machine = _context.Machines.Find(id);
            if (machine == null)
            {
                Console.WriteLine($"Machnie with id {id} not found");
                return NotFound();
            }
            _context.Machines.Remove(machine);
            _context.SaveChanges();
            return Ok("deleted");
        }

        [HttpPut]
        [Route("UpdateMachine")]
        public IActionResult UpdateMachine([FromBody] CreateMachineDto input, [FromQuery] int id)
        {
            Machine? machineToUpdate = _context.Machines.Find(id);
            if(machineToUpdate==null)
            {
                Console.WriteLine("Machine with this id does not exist");
                return NotFound();
            }
            
            machineToUpdate.Name = input.Name;
            machineToUpdate.Description = input.Description;
            _context.Machines.Update(machineToUpdate);
            _context.SaveChanges();
            return Ok(machineToUpdate);
        }
    }
}