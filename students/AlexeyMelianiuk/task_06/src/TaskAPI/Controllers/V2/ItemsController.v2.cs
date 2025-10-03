using Microsoft.AspNetCore.Mvc;
using TaskAPI.Models;
using TaskAPI.Models.V2;
using System.Net;

namespace TaskAPI.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class ItemsController : ControllerBase
{
    private static readonly List<ItemDto> _items = new()
    {
        new ItemDto 
        { 
            Id = 1, 
            Name = "Item 1 v2", 
            Description = "Description 1 with extended features", 
            Category = "Electronics",
            Price = 99.99m,
            Tags = new List<string> { "new", "featured" },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new ItemDto 
        { 
            Id = 2, 
            Name = "Item 2 v2", 
            Description = "Description 2 with extended features", 
            Category = "Books",
            Price = 29.99m,
            Tags = new List<string> { "bestseller" },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }
    };

    /// <summary>
    /// Получить список элементов с расширенной фильтрацией
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <response code="200">Возвращает список элементов</response>
    /// <response code="400">Некорректные параметры запроса</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<ItemDto>> GetItems([FromQuery] ItemQueryParameters parameters)
    {
        var query = _items.AsQueryable();
        
        // Базовая фильтрация
        if (!string.IsNullOrEmpty(parameters.Filter))
        {
            query = query.Where(i => i.Name.Contains(parameters.Filter, StringComparison.OrdinalIgnoreCase) ||
                                   i.Description.Contains(parameters.Filter, StringComparison.OrdinalIgnoreCase) ||
                                   i.Category.Contains(parameters.Filter, StringComparison.OrdinalIgnoreCase));
        }

        // Фильтрация по категории (новая функция в v2)
        if (!string.IsNullOrEmpty(parameters.Category))
        {
            query = query.Where(i => i.Category.Equals(parameters.Category, StringComparison.OrdinalIgnoreCase));
        }

        // Сортировка с дополнительными полями
        if (!string.IsNullOrEmpty(parameters.Sort))
        {
            query = parameters.Sort.ToLower() switch
            {
                "name" => parameters.SortDirection == "desc" 
                    ? query.OrderByDescending(i => i.Name) 
                    : query.OrderBy(i => i.Name),
                "id" => parameters.SortDirection == "desc" 
                    ? query.OrderByDescending(i => i.Id) 
                    : query.OrderBy(i => i.Id),
                "createdat" => parameters.SortDirection == "desc" 
                    ? query.OrderByDescending(i => i.CreatedAt) 
                    : query.OrderBy(i => i.CreatedAt),
                "price" => parameters.SortDirection == "desc" 
                    ? query.OrderByDescending(i => i.Price) 
                    : query.OrderBy(i => i.Price),
                "category" => parameters.SortDirection == "desc" 
                    ? query.OrderByDescending(i => i.Category) 
                    : query.OrderBy(i => i.Category),
                _ => query
            };
        }

        var totalCount = query.Count();
        var items = query
            .Skip((parameters.Page - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToList();

        // Расширенные заголовки пагинации
        Response.Headers.Append("X-Pagination-TotalCount", totalCount.ToString());
        Response.Headers.Append("X-Pagination-PageSize", parameters.PageSize.ToString());
        Response.Headers.Append("X-Pagination-CurrentPage", parameters.Page.ToString());
        Response.Headers.Append("X-Pagination-TotalPages", Math.Ceiling((double)totalCount / parameters.PageSize).ToString());
        Response.Headers.Append("X-Pagination-HasNext", (parameters.Page * parameters.PageSize < totalCount).ToString());

        return Ok(items);
    }

    /// <summary>
    /// Получить элемент по ID с расширенной информацией
    /// </summary>
    /// <param name="id">ID элемента</param>
    /// <response code="200">Элемент найден</response>
    /// <response code="404">Элемент не найден</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<ItemDto> GetItem(int id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        
        if (item == null)
        {
            return NotFound(new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "Element not found",
                Status = (int)HttpStatusCode.NotFound,
                Detail = $"Element with ID {id} not found in API v2.0",
                Instance = HttpContext.Request.Path,
                Extensions = { ["apiVersion"] = "2.0" }
            });
        }

        return Ok(item);
    }

    /// <summary>
    /// Создать новый элемент с расширенными полями
    /// </summary>
    /// <param name="item">Данные элемента</param>
    /// <response code="201">Элемент создан</response>
    /// <response code="400">Некорректные данные</response>
    [HttpPost]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public ActionResult<ItemDto> CreateItem([FromBody] ItemDto item)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Validation error in API v2.0",
                Status = (int)HttpStatusCode.BadRequest,
                Instance = HttpContext.Request.Path,
                Extensions = { ["apiVersion"] = "2.0" }
            });
        }

        item.Id = _items.Max(i => i.Id) + 1;
        item.CreatedAt = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;
        _items.Add(item);

        return CreatedAtAction(nameof(GetItem), new { id = item.Id, version = "2.0" }, item);
    }
}