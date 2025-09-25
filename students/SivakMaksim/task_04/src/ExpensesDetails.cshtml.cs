using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class ExpensesDetailsModel : PageModel
{
    public Expense? Expense { get; set; }

    public IActionResult OnGet(int id)
    {
        var expenses = new List<Expense>
        {
            new Expense { Id = 1, Title = "Продукты", Amount = 2500, Category = "Продукты", 
                        Date = DateTime.Now.AddDays(-1), Description = "Покупка продуктов на неделю" },
            new Expense { Id = 2, Title = "Бензин", Amount = 1500, Category = "Транспорт", 
                        Date = DateTime.Now.AddDays(-2), Description = "Заправка автомобиля" },
            new Expense { Id = 3, Title = "Кино", Amount = 500, Category = "Развлечения", 
                        Date = DateTime.Now.AddDays(-3), Description = "Посещение кинотеатра" }
        };

        Expense = expenses.FirstOrDefault(e => e.Id == id);
        
        if (Expense == null)
        {
            return NotFound();
        }

        return Page();
    }
}