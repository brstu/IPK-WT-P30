using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FinancialAccounting.API.DTOs.v1;
using FinancialAccounting.API.DTOs.v2;
using FinancialAccounting.API.Models;

namespace FinancialAccounting.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class ExpenseCategoriesController : ControllerBase
{
    private static List<ExpenseCategory> _categories = new();
    private static int _nextId = 1;

    // GET: api/v1/expensecategories
    [MapToApiVersion("1.0")]
    [HttpGet]
    public IActionResult GetV1()
    {
        var items = _categories.Select(c => new ExpenseCategoryDtoV1
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            CreatedAt = c.CreatedAt
        }).ToList();

        return Ok(items);
    }

    // GET: api/v2/expensecategories
    [MapToApiVersion("2.0")]
    [HttpGet]
    public IActionResult GetV2()
    {
        var items = _categories.Select(c => new ExpenseCategoryDtoV2
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            MonthlyBudget = 0,
            ColorCode = "#3498db",
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        }).ToList();

        return Ok(items);
    }

    // POST: api/v2/expensecategories
    [MapToApiVersion("2.0")]
    [HttpPost]
    public IActionResult CreateV2([FromBody] ExpenseCategoryDtoV2 createDto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var category = new ExpenseCategory
        {
            Id = _nextId++,
            Name = createDto.Name,
            Description = createDto.Description,
            CreatedAt = DateTime.UtcNow
        };

        _categories.Add(category);

        var resultDto = new ExpenseCategoryDtoV2
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            MonthlyBudget = createDto.MonthlyBudget,
            ColorCode = createDto.ColorCode,
            CreatedAt = category.CreatedAt
        };

        return CreatedAtAction(nameof(GetByIdV2), new { id = category.Id, version = "2.0" }, resultDto);
    }

    // GET: api/v2/expensecategories/5
    [MapToApiVersion("2.0")]
    [HttpGet("{id}")]
    public IActionResult GetByIdV2(int id)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id);
        if (category == null)
        {
            return Problem(
                title: "Не найдено",
                detail: $"Категория с ID {id} не найдена",
                statusCode: StatusCodes.Status404NotFound);
        }

        var dto = new ExpenseCategoryDtoV2
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            MonthlyBudget = 0,
            ColorCode = "#3498db",
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };

        return Ok(dto);
    }
}
