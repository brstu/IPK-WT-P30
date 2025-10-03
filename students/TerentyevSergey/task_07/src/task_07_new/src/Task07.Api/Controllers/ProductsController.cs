using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Cryptography;
using Task07.Api.Models;
using Microsoft.Extensions.Primitives;

namespace Task07.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ResponseCache(Duration = 30)]
    public class ProductsController : ControllerBase
    {
        private static readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Laptop", Description = "Gaming laptop", Price = 999.99m, Stock = 10 },
            new Product { Id = 2, Name = "Mouse", Description = "Wireless mouse", Price = 29.99m, Stock = 50 },
            new Product { Id = 3, Name = "Keyboard", Description = "Mechanical keyboard", Price = 89.99m, Stock = 30 }
        };

        [HttpGet]
        public IActionResult GetProducts()
        {
            // Генерируем ETag на основе содержимого
            var content = System.Text.Json.JsonSerializer.Serialize(_products);
            var etag = GenerateETag(content);

            // Правильное сравнение StringValues
            if (Request?.Headers.IfNoneMatch.Count > 0 &&
                Request.Headers.IfNoneMatch[0] == etag)
            {
                return StatusCode(304); // Not Modified
            }

            // Добавляем заголовки только если Response доступен
            if (Response != null)
            {
                Response.Headers.ETag = etag;
                Response.Headers.CacheControl = "public, max-age=30";
            }

            return Ok(_products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var content = System.Text.Json.JsonSerializer.Serialize(product);
            var etag = GenerateETag(content);

            if (Request?.Headers.IfNoneMatch.Count > 0 &&
                Request.Headers.IfNoneMatch[0] == etag)
            {
                return StatusCode(304);
            }

            if (Response != null)
            {
                Response.Headers.ETag = etag;
                Response.Headers.CacheControl = "public, max-age=30";
            }

            return Ok(product);
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            product.Id = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _products.Remove(product);
            return NoContent();
        }

        private static string GenerateETag(string content)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(content));
            return $"\"{Convert.ToBase64String(bytes)}\"";
        }
        
        [HttpGet("error-test")]
        public IActionResult ErrorTest()
        {
    // Искусственно создаем ошибку
    throw new InvalidOperationException("This is a test error for ProblemDetails");
        }
    }
}