using Microsoft.AspNetCore.Mvc;
using Task06.API.Models.V2;

namespace Task06.API.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ItemsController : ControllerBase
{
    private static readonly List<ItemDto> _items = new();
    private static int _nextId = 1;

    [HttpGet]
    public ActionResult<IEnumerable<ItemDto>> GetItems(
        [FromQuery] string? filter,
        [FromQuery] string? category,
        [FromQuery] string? sort = "name",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = _items.AsQueryable();
        
        // Фильтрация по названию/описанию
        if (!string.IsNullOrEmpty(filter))
        {
            query = query.Where(i => i.Name.Contains(filter, StringComparison.OrdinalIgnoreCase) || 
                                   i.Description.Contains(filter, StringComparison.OrdinalIgnoreCase));
        }

        // Новая фильтрация в v2 - по категории
        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(i => i.Category == category);
        }

        // Сортировка
        query = sort?.ToLower() switch
        {
            "price" => query.OrderBy(i => i.Price),
            "date" => query.OrderBy(i => i.CreatedAt),
            "rating" => query.OrderByDescending(i => i.Rating), // Новая сортировка в v2
            _ => query.OrderBy(i => i.Name)
        };

        // Пагинация
        var totalCount = query.Count();
        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Ok(new
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        });
    }

    [HttpGet("{id}")]
    public ActionResult<ItemDto> GetItem(int id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        if (item == null)
        {
            return NotFound(new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "Item not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Item with id {id} was not found",
                Instance = HttpContext.Request.Path
            });
        }

        return Ok(item);
    }

    [HttpPost]
    public ActionResult<ItemDto> CreateItem(CreateItemDto createDto)
    {
        var item = new ItemDto
        {
            Id = _nextId++,
            Name = createDto.Name,
            Description = createDto.Description,
            Price = createDto.Price,
            Category = createDto.Category,
            Rating = createDto.Rating,
            Tags = createDto.Tags ?? new List<string>(),
            CreatedAt = DateTime.UtcNow
        };

        _items.Add(item);
        
        return CreatedAtAction(nameof(GetItem), new { id = item.Id, version = "2" }, item);
    }

    [HttpPut("{id}")]
    public ActionResult<ItemDto> UpdateItem(int id, UpdateItemDto updateDto)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        if (item == null)
        {
            return NotFound(new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "Item not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Item with id {id} was not found",
                Instance = HttpContext.Request.Path
            });
        }

        item.Name = updateDto.Name;
        item.Description = updateDto.Description;
        item.Price = updateDto.Price;
        item.Category = updateDto.Category;
        item.Rating = updateDto.Rating;
        item.Tags = updateDto.Tags ?? new List<string>();
        item.UpdatedAt = DateTime.UtcNow;

        return Ok(item);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteItem(int id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        if (item == null)
        {
            return NotFound(new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "Item not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Item with id {id} was not found",
                Instance = HttpContext.Request.Path
            });
        }

        _items.Remove(item);
        return NoContent();
    }

    // Новый метод в v2 - поиск по тегам
    [HttpGet("search/tags")]
    public ActionResult<IEnumerable<ItemDto>> GetItemsByTags([FromQuery] List<string> tags)
    {
        if (tags == null || !tags.Any())
        {
            return BadRequest(new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Invalid request",
                Status = StatusCodes.Status400BadRequest,
                Detail = "At least one tag must be provided",
                Instance = HttpContext.Request.Path
            });
        }

        var items = _items.Where(i => i.Tags.Any(tag => tags.Contains(tag)))
                         .ToList();

        return Ok(items);
    }
}