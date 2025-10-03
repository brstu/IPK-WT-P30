using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task04.Web.Pages
{
    public class ProtectedModel : PageModel
    {
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "1";
            Username = HttpContext.Session.GetString("Username") ?? "Пользователь";

            if (!IsAuthenticated)
            {
                // Можно также сделать редирект на страницу логина
                // return RedirectToPage("/Login");
            }

            return Page();
        }
    }
}