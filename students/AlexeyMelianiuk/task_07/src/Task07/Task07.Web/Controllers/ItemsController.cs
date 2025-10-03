using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Task07.Application.Interfaces;
using Task07.Domain.Entities;

namespace Task07.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[ResponseCache(Duration = 60)] // Кэширование на 60 секунд
public class ItemsController : ControllerBase
{
    private readonly IItemService _itemService;

    public ItemsController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Item>>> GetItems()
    {
        var items = await _itemService.GetAllItemsAsync();
        
        // Генерация ETag на основе содержимого
        var content = System.Text.Json.JsonSerializer.Serialize(items);
        var etag = GenerateETag(content);
        
        // Проверка If-None-Match
        if (Request.Headers.IfNoneMatch.ToString().Contains(etag))
        {
            return StatusCode(304); // Not Modified
        }
        
        Response.Headers.ETag = etag;
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Item>> GetItem(int id)
    {
        var item = await _itemService.GetItemByIdAsync(id);
        
        if (item == null)
        {
            return NotFound(new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "Item not found",
                Status = 404,
                Detail = $"Item with ID {id} not found",
                Instance = HttpContext.Request.Path
            });
        }

        var content = System.Text.Json.JsonSerializer.Serialize(item);
        var etag = GenerateETag(content);
        
        if (Request.Headers.IfNoneMatch.ToString().Contains(etag))
        {
            return StatusCode(304);
        }
        
        Response.Headers.ETag = etag;
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Item>> CreateItem(Item item)
    {
        try
        {
            var createdItem = await _itemService.CreateItemAsync(item);
            return CreatedAtAction(nameof(GetItem), new { id = createdItem.Id }, createdItem);
        }
        catch (Exception ex)
        {
            return BadRequest(new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Validation error",
                Status = 400,
                Detail = ex.Message,
                Instance = HttpContext.Request.Path
            });
        }
    }

    private static string GenerateETag(string content)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(content));
        return $"\"{Convert.ToBase64String(hash)}\"";
    }
}
