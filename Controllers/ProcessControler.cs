using Microsoft.AspNetCore.Mvc;
using MiniMES.Models;
using Microsoft.EntityFrameworkCore;

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

        public class ProcessDto
        {
            public int Id { get; set; }
            public int SerialNumber { get; set; }
            public int OrderId { get; set; }
            public string Status { get; set; } = "";
            public DateTime DateTime { get; set; }
        }

        public class CreateProcessDto
        {
            public int SerialNumber { get; set; }
            public int OrderId { get; set; }
            public string Status { get; set; } = null!;
        }
        
        [HttpPost]
        [Route("AddProcess")]
        public IActionResult AddProcesse([FromBody] CreateProcessDto input)
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
        
    }
    
}