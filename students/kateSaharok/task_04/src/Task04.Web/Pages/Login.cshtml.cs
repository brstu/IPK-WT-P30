using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task04.Web.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; } = string.Empty;
        
        [BindProperty]
        public string Password { get; set; } = string.Empty;
        
        public string Message { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Фиктивная аутентификация
            if (Username == "admin" && Password == "password")
            {
                HttpContext.Session.SetString("IsAuthenticated", "true");
                HttpContext.Session.SetString("Username", Username);
                return RedirectToPage("/Protected");
            }
            else
            {
                Message = "Неверные учетные данные. Попробуйте admin/password";
                return Page();
            }
        }
    }
}