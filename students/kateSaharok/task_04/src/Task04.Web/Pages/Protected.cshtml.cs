using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task04.Web.Pages
{
    public class ProtectedModel : PageModel
    {
        public string Username { get; set; } = "Гость";
        public string LoginTime { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            var isAuthenticated = HttpContext.Session.GetString("IsAuthenticated");
            if (string.IsNullOrEmpty(isAuthenticated))
            {
                return RedirectToPage("/Login");
            }
            
            Username = HttpContext.Session.GetString("Username") ?? "Пользователь";
            LoginTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            return Page();
        }
    }
}