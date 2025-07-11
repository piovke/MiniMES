using Microsoft.AspNetCore.Mvc;
using MiniMES.Models;


namespace MiniMES.Controllers

{
    [Route("MiniMES")]
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
        public IActionResult AddMachine([FromBody] Machine machine)
        {
            if (machine == null)
            {
                return BadRequest();
            }
            
            _context.Machines.Add(machine);
            _context.SaveChanges();
            return Ok($"added \"{machine.Name}\"");
        }

        [HttpGet]
        [Route("ShowMachines")]
        public IActionResult ShowMachines()
        {
            try
            {
                if (!_context.Machines.Any())
                {
                    return NoContent();
                }

                var machines = _context.Machines.ToList();
                return Ok(machines);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}