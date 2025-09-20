using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LoginModel : PageModel
{
    [BindProperty]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("IsAuthenticated") == "true")
        {
            return RedirectToPage("/Budget");
        }
        return Page();
    }

    public IActionResult OnPost()
    {
        if (Username == "admin" && Password == "password")
        {
            HttpContext.Session.SetString("IsAuthenticated", "true");
            HttpContext.Session.SetString("Username", Username);
            return RedirectToPage("/Budget");
        }
        else
        {
            ErrorMessage = "Неверные учетные данные!";
            return Page();
        }
    }
}