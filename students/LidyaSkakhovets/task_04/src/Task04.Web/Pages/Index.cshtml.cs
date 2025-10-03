using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task04.Web.Pages
{
    public class IndexModel : PageModel
    {
        public bool IsAuthenticated { get; set; }

        public void OnGet()
        {
            IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "1";
        }
    }
}