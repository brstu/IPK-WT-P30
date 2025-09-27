using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
    {
        var expenses = await _expenseService.GetExpensesAsync();
        var etag = GenerateETag(expenses);
        
        if (Request.Headers.IfNoneMatch == etag)
            return StatusCode(304);
        
        Response.Headers.ETag = etag;
        return Ok(expenses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Expense>> GetExpense(int id)
    {
        var expense = await _expenseService.GetExpenseByIdAsync(id);
        if (expense == null) return NotFound();
        return Ok(expense);
    }

    [HttpPost]
    public async Task<ActionResult<Expense>> CreateExpense(Expense expense)
    {
        await _expenseService.AddExpenseAsync(expense);
        return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense);
    }

    private static string GenerateETag(IEnumerable<Expense> expenses)
    {
        var content = string.Join("", expenses.Select(e => $"{e.Id}{e.Amount}"));
        var bytes = Encoding.UTF8.GetBytes(content);
        var hash = SHA256.HashData(bytes);
        return $"\"{Convert.ToBase64String(hash)}\"";
    }
}
