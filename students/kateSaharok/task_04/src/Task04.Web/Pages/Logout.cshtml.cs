using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task04.Web.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("IsAuthenticated");
            HttpContext.Session.Remove("Username");
            return RedirectToPage("/Index");
        }
    }
}