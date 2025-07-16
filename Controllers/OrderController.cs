using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniMES.Models;
using MiniMES.DTOs;

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
        
        [HttpGet]
        [Route("ShowOrders")]
        public IActionResult ShowOrders()
        {
            var orderDtos = _context.Orders
                .Include(o => o.Machine)
                .Include(o => o.Product)
                .Include(o=>o.Processes)
                .Select(o=>new OrderDto
                {
                    Id = o.Id,
                    Code = o.Code,
                    Machine = new MachineDto
                    {
                        Id = o.Machine.Id,
                        Name = o.Machine.Name,
                    },
                    Product = new ProductDto()
                    {
                        Id = o.Product.Id,
                        Name = o.Product.Name,
                    },
                    Processes = o.Processes.Select(p=> new ProcessDto
                    {
                        SerialNumber = p.SerialNumber,
                        
                    }).ToList(),
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

        [HttpPut]
        [Route("UpdateOrder")]
        public IActionResult UpdateOrder([FromBody] CreateOrderDto input, [FromQuery] int id)
        {
            var orderToUpdate = _context.Orders.Find(id);
            if (orderToUpdate == null)
            {
                return BadRequest("Order not found");
            }
            orderToUpdate.Code = input.Code;
            orderToUpdate.MachineId = input.MachineId;
            orderToUpdate.ProductId = input.ProductId;
            orderToUpdate.Quantity = input.Quantity;
            _context.Orders.Update(orderToUpdate);
            _context.SaveChanges();
            return Ok("Order updated");
        }

        [HttpDelete]
        [Route("DeleteOrder")]
        public IActionResult DeleteOrder([FromQuery] int id)
        {
            var orderToDelete = _context.Orders.Find(id);
            if (orderToDelete == null)
            {
                return BadRequest();
            }
            _context.Orders.Remove(orderToDelete);
            _context.SaveChanges();
            return Ok("Deleted");
        }

        [HttpGet]
        [Route("Details/{id}")]
        public IActionResult Details([FromRoute] int id)
        {
            var order = _context.Orders
                .Include(o => o.Machine)
                .Include(o => o.Product)
                .Include(o => o.Processes)
                .FirstOrDefault(o=>o.Id == id);
            if (order == null)
            {
                return BadRequest();
            }
            var orderDto = new OrderDto
                {
                    Id = order.Id,
                    Code = order.Code,
                    Machine = new MachineDto
                    {
                        Id = order.Machine.Id,
                        Name = order.Machine.Name,
                    },
                    Product = new ProductDto()
                    {
                        Id = order.Product.Id,
                        Name = order.Product.Name,
                    },
                    Processes = order.Processes.Select(p=> new ProcessDto
                    {
                        Id = p.Id,
                        SerialNumber = p.SerialNumber,
                        
                    }).ToList(),
                    Quantity = order.Quantity
                };
            return Ok(orderDto);
        }
    }
}