using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task04.Web.Pages;

public class SecureModel : PageModel
{
    public IActionResult OnGet()
    {
        // Check authentication
        if (HttpContext.Session.GetString("IsAuthenticated") != "true")
        {
            return RedirectToPage("/Login");
        }

        return Page();
    }
}