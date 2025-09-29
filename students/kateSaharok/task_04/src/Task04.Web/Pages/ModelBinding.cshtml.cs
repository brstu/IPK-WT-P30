using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task04.Web.Pages
{
    public class ModelBindingModel : PageModel
    {
        // Данные из формы (POST)
        [BindProperty]
        public string Name { get; set; } = string.Empty;
        
        [BindProperty]
        public string Email { get; set; } = string.Empty;
        
        [BindProperty]
        public int Age { get; set; }

        // Данные из query string (GET)
        [FromQuery]
        public int QueryId { get; set; }
        
        [FromQuery(Name = "category")]
        public string Category { get; set; } = string.Empty;

        // Данные из route (GET)
        [FromRoute]
        public int? RouteId { get; set; }

        public bool ShowResults { get; set; }

        public void OnGet(int? id)
        {
            RouteId = id;
            ShowResults = !string.IsNullOrEmpty(Category) || QueryId > 0 || RouteId.HasValue;
        }

        public IActionResult OnPost()
        {
            // Данные автоматически привязываются через [BindProperty]
            ShowResults = true;
            return Page();
        }
    }
}