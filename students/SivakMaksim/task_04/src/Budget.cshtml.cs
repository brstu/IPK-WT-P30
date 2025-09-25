using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class BudgetModel : PageModel
{
    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("IsAuthenticated") != "true")
        {
            return RedirectToPage("/Login");
        }

        return Page();
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.Session.Remove("IsAuthenticated");
        HttpContext.Session.Remove("Username");
        return RedirectToPage("/Login");
    }
}