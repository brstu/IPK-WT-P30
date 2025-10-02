using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task04.Web.Pages.Items
{
    public class DetailsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public int ItemId => Id;

        public void OnGet()
        {
        }
    }
}