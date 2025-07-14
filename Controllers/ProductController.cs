using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniMES.Models;

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

        public class ProductDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public string Description { get; set; } = "";
            public List<int> OrderIds { get; set; } = new();
        }

        public class CreateProductDto
        {
            public string Name { get; set; } = "";
            public string Description { get; set; } = "";
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
