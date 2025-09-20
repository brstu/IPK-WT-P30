using Microsoft.AspNetCore.Mvc.RazorPages;

public class CategoriesModel : PageModel
{
    public List<string> Categories { get; set; } = new List<string>
    {
        "Продукты",
        "Транспорт",
        "Жилье",
        "Развлечения",
        "Здоровье",
        "Образование"
    };

    public void OnGet()
    {
    }
}