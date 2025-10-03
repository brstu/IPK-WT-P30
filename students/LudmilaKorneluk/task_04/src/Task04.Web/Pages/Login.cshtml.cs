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

        public bool ShowMessage { get; set; }
        public string Message { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Фиктивная аутентификация
            if (Username == "admin" && Password == "password")
            {
                HttpContext.Session.SetString("IsAuthenticated", "1");
                HttpContext.Session.SetString("Username", Username);
                
                ShowMessage = true;
                Message = "Успешный вход!";
                
                return RedirectToPage("/Index");
            }
            else
            {
                ShowMessage = true;
                Message = "Неверные учетные данные!";
                return Page();
            }
        }
    }
}