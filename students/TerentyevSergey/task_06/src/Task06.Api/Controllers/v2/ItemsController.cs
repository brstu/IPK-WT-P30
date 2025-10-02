using Microsoft.AspNetCore.Mvc;
using Task06.Api.Models;

namespace Task06.Api.Controllers.v2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ItemsController : ControllerBase
{
    private static List<ItemDtoV2> _items = new()
    {
        new ItemDtoV2 { Id = 1, Name = "Item 1", Description = "Description 1", Price = 10.99m, Category = "Electronics", CreatedAt = DateTime.UtcNow },
        new ItemDtoV2 { Id = 2, Name = "Item 2", Description = "Description 2", Price = 20.50m, Category = "Books", CreatedAt = DateTime.UtcNow }
    };

    [HttpGet]
    public IActionResult GetItems([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string sort = "name", [FromQuery] string filter = "")
    {
        var query = _items.AsQueryable();
        
        // Фильтрация
        if (!string.IsNullOrEmpty(filter))
        {
            query = query.Where(x => 
                x.Name.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                x.Category.Contains(filter, StringComparison.OrdinalIgnoreCase));
        }
        
        // Сортировка
        query = sort.ToLower() switch
        {
            "price" => query.OrderBy(x => x.Price),
            "price_desc" => query.OrderByDescending(x => x.Price),
            "category" => query.OrderBy(x => x.Category),
            "date" => query.OrderByDescending(x => x.CreatedAt),
            _ => query.OrderBy(x => x.Name)
        };
        
        // Пагинация
        var totalCount = query.Count();
        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            
        return Ok(new { items, totalCount, page, pageSize, filter, sort });
    }

    [HttpGet("{id}")]
    public IActionResult GetItem(int id)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        if (item == null)
        {
            return Problem(
                title: "Item not found",
                detail: $"Item with id {id} was not found",
                statusCode: 404);
        }
        return Ok(item);
    }
}