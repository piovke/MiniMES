using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniMES.Models;

namespace MiniMES.Controllers

{
    [Route("Orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly MiniProductionDbContext _context;

        public OrderController(MiniProductionDbContext context)
        {
            _context = context;
        }

        public class OrderDto
        {
            public int OrderId { get; set; }
            public string Code { get; set; } ="";
            // public int MachineId { get; set; }
            public string MachineName { get; set; }="";
            // public int ProductId { get; set; }
            public string ProductName { get; set; }="";
            public int Quantity { get; set; }
        }

        public class CreateOrderDto
        {
            public string Code { get; set; } ="";
            public int MachineId { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        [HttpGet]
        [Route("ShowOrders")]
        public IActionResult ShowOrders()
        {
            var orderDtos = _context.Orders
                .Include(o => o.Machine)
                .Include(o => o.Product)
                .Select(o=>new OrderDto
                {
                    Code = o.Code,
                    // MachineId = o.MachineId,
                    MachineName = o.Machine.Name,
                    // ProductId = o.ProductId,
                    ProductName = o.Product.Name,
                    Quantity = o.Quantity
                })
                .ToList();
            if (_context.Orders.Any() == false)
            {
                Console.WriteLine("no orders to show");
                return NoContent();
            }
            return Ok(orderDtos);
            
        }

        [HttpPost]
        [Route("CreateOrder")]
        public IActionResult CreateOrder([FromBody] CreateOrderDto input)
        {
            if (!_context.Machines.Any(m => m.Id == input.MachineId) ||
                !_context.Products.Any(p => p.Id == input.ProductId))
            {
                return BadRequest("Invalid MachineId or ProductId.");
            }

            Order order = new Order
            {
                Code = input.Code,
                MachineId = input.MachineId,
                ProductId = input.ProductId,
                Quantity = input.Quantity
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            return Ok("Order created");
        }
    }
}