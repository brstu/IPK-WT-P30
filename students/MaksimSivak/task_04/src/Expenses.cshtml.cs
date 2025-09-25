using Microsoft.AspNetCore.Mvc.RazorPages;

public class ExpensesModel : PageModel
{
    public List<Expense> Expenses { get; set; } = new List<Expense>
    {
        new Expense { Id = 1, Title = "Продукты", Amount = 2500, Category = "Продукты", Date = DateTime.Now.AddDays(-1) },
        new Expense { Id = 2, Title = "Бензин", Amount = 1500, Category = "Транспорт", Date = DateTime.Now.AddDays(-2) },
        new Expense { Id = 3, Title = "Кино", Amount = 500, Category = "Развлечения", Date = DateTime.Now.AddDays(-3) }
    };

    public void OnGet()
    {
    }
}