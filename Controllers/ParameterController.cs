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
            return Ok(parameter);
        }
    }
}