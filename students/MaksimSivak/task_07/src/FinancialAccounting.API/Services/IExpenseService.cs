using System.Collections.Generic;
using System.Threading.Tasks;

public interface IExpenseService
{
    Task<IEnumerable<Expense>> GetExpensesAsync();
    Task<Expense> GetExpenseByIdAsync(int id);
    Task AddExpenseAsync(Expense expense);
}
