using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniMES.Models;
using MiniMES.DTOs;
namespace MiniMES.Controllers

{
    [Route("Products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly MiniProductionDbContext _context;

        public ProductController(MiniProductionDbContext context)
        {
            _context = context;
        }
        
        [HttpPost]
        [Route("AddProduct")]
        public IActionResult AddProduct([FromBody] CreateProductDto input)
        {
            Product product = new Product()
            {
                Name = input.Name,
                Description = input.Description
            };
            
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok($"added \"{product.Name}\"");
        }

        [HttpGet]
        [Route("ShowProducts")]
        public IActionResult ShowProducts()
        {
            if (!_context.Products.Any())
            {
                return NoContent();
            }
            
            var productsDtos = _context.Products
                .Include(p=>p.Orders)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    OrderIds = p.Orders.Select(o=>o.Id).ToList()
                }).ToList();
            
            return Ok(productsDtos);
        }
        
        [HttpPut]
        [Route("UpdateProducts")]
        public IActionResult UpdateProduct([FromBody] CreateProductDto input, [FromQuery] int id)
        {
            Product? productToUpdate = _context.Products.Find(id);
            if(productToUpdate==null)
            {
                Console.WriteLine("Product with this id does not exist");
                return NotFound();
            }
            
            productToUpdate.Name = input.Name;
            productToUpdate.Description = input.Description;
            _context.Products.Update(productToUpdate);
            _context.SaveChanges();
            return Ok(productToUpdate);
        }

        [HttpDelete]
        [Route("DeleteProduct")]
        public IActionResult DeleteProduct([FromQuery] int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound("No such product");
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return Ok("Product deleted");
        }
    }
}
