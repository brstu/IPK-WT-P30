using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task04.Web.Pages;

public class IndexModel : PageModel
{
    public bool IsAuthenticated { get; private set; }

    public void OnGet()
    {
        IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "true";
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.Session.Remove("IsAuthenticated");
        return RedirectToPage("/Index");
    }
}