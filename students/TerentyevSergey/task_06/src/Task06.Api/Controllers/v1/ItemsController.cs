using Microsoft.AspNetCore.Mvc;
using Task06.Api.Models;

namespace Task06.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ItemsController : ControllerBase
{
    private static List<ItemDto> _items = new()
    {
        new ItemDto { Id = 1, Name = "Item 1", Description = "Description 1", Price = 10.99m },
        new ItemDto { Id = 2, Name = "Item 2", Description = "Description 2", Price = 20.50m }
    };

    [HttpGet]
    public IActionResult GetItems([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string sort = "name")
    {
        // Пагинация и сортировка
        var query = _items.AsQueryable();
        
        // Сортировка
        query = sort.ToLower() switch
        {
            "price" => query.OrderBy(x => x.Price),
            "price_desc" => query.OrderByDescending(x => x.Price),
            _ => query.OrderBy(x => x.Name)
        };
        
        // Пагинация
        var totalCount = query.Count();
        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            
        return Ok(new { items, totalCount, page, pageSize });
    }

    [HttpGet("{id}")]
    public IActionResult GetItem(int id)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    [HttpPost]
    public IActionResult CreateItem([FromBody] CreateItemRequest request)
    {
        var newItem = new ItemDto
        {
            Id = _items.Max(x => x.Id) + 1,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price
        };
        
        _items.Add(newItem);
        return CreatedAtAction(nameof(GetItem), new { id = newItem.Id }, newItem);
    }
}