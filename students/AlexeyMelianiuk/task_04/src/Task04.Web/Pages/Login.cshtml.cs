using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task04.Web.Pages;

public class LoginModel : PageModel
{
    [BindProperty]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public string Message { get; private set; } = string.Empty;

    public IActionResult OnGet()
    {
        // If already authenticated, redirect to home
        if (HttpContext.Session.GetString("IsAuthenticated") == "true")
            return RedirectToPage("/Index");
        
        return Page();
    }

    public IActionResult OnPost()
    {
        // Simple authentication without database
        if (Username == "admin" && Password == "password")
        {
            HttpContext.Session.SetString("IsAuthenticated", "true");
            return RedirectToPage("/Index");
        }
        
        Message = "Неверные учетные данные. Используйте admin/password";
        return Page();
    }
}