using Microsoft.AspNetCore.Mvc;
using MiniMES.Models;
using Microsoft.EntityFrameworkCore;
using MiniMES.DTOs;


namespace MiniMES.Controllers

{
    [Route("Parameters")]
    [ApiController]
    public class ParameterController : ControllerBase
    {
        private readonly MiniProductionDbContext _context;

        public ParameterController(MiniProductionDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("AddParameter")]
        public IActionResult AddParameter([FromBody] CreateParameterDto input)
        {
            if (input.Name.Length == 0 || input.Unit.Length == 0)
            {
                return BadRequest("name or unit not provided");
            }
            
            Parameter parameter = new Parameter()
            {
                Name = input.Name,
                Unit = input.Unit,
            };
            
            _context.Parameters.Add(parameter);
            _context.SaveChanges();
            return Ok("added");
        }

        [HttpGet]
        [Route("GetParameters")]
        public IActionResult GetParameters()
        {
            var parameters = _context.Parameters
                .Include(p=>p.ProcessParameters)
                .Select(parameter => new ParameterDto
                {
                    Id = parameter.Id,
                    Name = parameter.Name,
                    Unit = parameter.Unit,
                }).ToList();

            if (!parameters.Any())
            {
                return NoContent();
            }
            return Ok(parameters);
        }

        [HttpPut]
        [Route("UpdateParameter")]
        public IActionResult UpdateParameter([FromQuery] int id, [FromBody] CreateParameterDto input)
        {

            Parameter? parameterToUpdate = _context.Parameters.Find(id);
            if (parameterToUpdate == null)
            {
                return NotFound();
            }
            parameterToUpdate.Name = input.Name;
            parameterToUpdate.Unit = input.Unit;
            _context.Parameters.Update(parameterToUpdate);
            _context.SaveChanges();
            return Ok("updated");
        }

        [HttpDelete]
        [Route("DeleteParameter")]
        public IActionResult DeleteParameter([FromQuery] int id)
        {
            Parameter? parameterToDelete = _context.Parameters.Find(id);
            if (parameterToDelete == null)
            {
                return NotFound();
            }
            _context.Parameters.Remove(parameterToDelete);
            _context.SaveChanges();
            return Ok("deleted");
        }
    }
}