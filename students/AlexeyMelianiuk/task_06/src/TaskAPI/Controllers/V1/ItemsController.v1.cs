using Microsoft.AspNetCore.Mvc;
using TaskAPI.Models;
using TaskAPI.Models.V1;
using System.Net;

namespace TaskAPI.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class ItemsController : ControllerBase
{
    private static readonly List<ItemDto> _items = new()
    {
        new ItemDto { Id = 1, Name = "Item 1", Description = "Description 1", CreatedAt = DateTime.UtcNow },
        new ItemDto { Id = 2, Name = "Item 2", Description = "Description 2", CreatedAt = DateTime.UtcNow },
        new ItemDto { Id = 3, Name = "Test Item", Description = "Description 3", CreatedAt = DateTime.UtcNow }
    };

    /// <summary>
    /// Получить список элементов с пагинацией и фильтрацией
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <response code="200">Возвращает список элементов</response>
    /// <response code="400">Некорректные параметры запроса</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<ItemDto>> GetItems([FromQuery] ItemQueryParameters parameters)
    {
        // Применение фильтрации
        var query = _items.AsQueryable();
        
        if (!string.IsNullOrEmpty(parameters.Filter))
        {
            query = query.Where(i => i.Name.Contains(parameters.Filter, StringComparison.OrdinalIgnoreCase) ||
                                   i.Description.Contains(parameters.Filter, StringComparison.OrdinalIgnoreCase));
        }

        // Применение сортировки
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
                _ => query
            };
        }

        // Применение пагинации
        var totalCount = query.Count();
        var items = query
            .Skip((parameters.Page - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToList();

        // Добавление заголовков пагинации
        Response.Headers.Append("X-Pagination-TotalCount", totalCount.ToString());
        Response.Headers.Append("X-Pagination-PageSize", parameters.PageSize.ToString());
        Response.Headers.Append("X-Pagination-CurrentPage", parameters.Page.ToString());
        Response.Headers.Append("X-Pagination-TotalPages", Math.Ceiling((double)totalCount / parameters.PageSize).ToString());

        return Ok(items);
    }

    /// <summary>
    /// Получить элемент по ID
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
                Detail = $"Element with ID {id} not found",
                Instance = HttpContext.Request.Path
            });
        }

        return Ok(item);
    }

    /// <summary>
    /// Создать новый элемент
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
                Title = "Validation error",
                Status = (int)HttpStatusCode.BadRequest,
                Instance = HttpContext.Request.Path
            });
        }

        item.Id = _items.Max(i => i.Id) + 1;
        item.CreatedAt = DateTime.UtcNow;
        _items.Add(item);

        return CreatedAtAction(nameof(GetItem), new { id = item.Id, version = "1.0" }, item);
    }
}