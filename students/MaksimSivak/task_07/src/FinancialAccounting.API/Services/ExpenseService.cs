using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ExpenseService : IExpenseService
{
    private readonly List<Expense> _expenses = new();
    private int _nextId = 1;

    public Task<IEnumerable<Expense>> GetExpensesAsync()
    {
        return Task.FromResult(_expenses.AsEnumerable());
    }

    public Task<Expense> GetExpenseByIdAsync(int id)
    {
        return Task.FromResult(_expenses.FirstOrDefault(e => e.Id == id));
    }

    public Task AddExpenseAsync(Expense expense)
    {
        expense.Id = _nextId++;
        expense.Date = DateTime.Now;
        _expenses.Add(expense);
        return Task.CompletedTask;
    }
}
