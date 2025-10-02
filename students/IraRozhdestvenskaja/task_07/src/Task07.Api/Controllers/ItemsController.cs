using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Task07.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private static readonly List<Item> _items = new()
    {
        new Item { Id = 1, Name = "Item 1", Description = "Description 1" },
        new Item { Id = 2, Name = "Item 2", Description = "Description 2" },
        new Item { Id = 3, Name = "Item 3", Description = "Description 3" }
    };

    [HttpGet]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public IActionResult GetItems()
    {
        try
        {
            var content = System.Text.Json.JsonSerializer.Serialize(_items);
            var etag = ComputeEtag(content);
            
            // Проверяем If-None-Match
            if (Request.Headers.IfNoneMatch == etag)
            {
                return StatusCode(304); // Not Modified
            }
            
            Response.Headers.ETag = etag;
            Response.Headers.CacheControl = "public, max-age=60";
            return Ok(_items);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
    public IActionResult GetItem(int id)
    {
        try
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound(new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Title = "Not Found",
                    Status = 404,
                    Detail = $"Item with id {id} not found",
                    Instance = $"/api/items/{id}"
                });
            }

            var content = System.Text.Json.JsonSerializer.Serialize(item);
            var etag = ComputeEtag(content);
            
            // Проверяем If-None-Match
            if (Request.Headers.IfNoneMatch == etag)
            {
                return StatusCode(304); // Not Modified
            }
            
            Response.Headers.ETag = etag;
            Response.Headers.CacheControl = "public, max-age=30";
            return Ok(item);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpPost]
    public IActionResult CreateItem(Item item)
    {
        if (item == null || string.IsNullOrEmpty(item.Name))
        {
            return BadRequest(new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Bad Request",
                Status = 400,
                Detail = "Item name is required",
                Instance = "/api/items"
            });
        }

        item.Id = _items.Max(i => i.Id) + 1;
        _items.Add(item);
        return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
    }

    [HttpGet("error")]
    public IActionResult ThrowError()
    {
        throw new Exception("This is a test exception for error handling");
    }

    private static string ComputeEtag(string content)
    {
        try
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(content));
            return $"\"{Convert.ToBase64String(hash)}\"";
        }
        catch
        {
            // Fallback: use content length as simple ETag
            return $"\"{content.GetHashCode()}\"";
        }
    }
}

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}