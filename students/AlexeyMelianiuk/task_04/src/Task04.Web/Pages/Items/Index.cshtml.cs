using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task04.Web.Pages.Items;

public class IndexModel : PageModel
{
    private static readonly List<Item> _items = new()
    {
        new Item(1, "Элемент 1", "Описание первого элемента"),
        new Item(2, "Элемент 2", "Описание второго элемента"),
        new Item(3, "Элемент 3", "Описание третьего элемента")
    };

    public List<Item> Items { get; private set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Category { get; set; }

    public void OnGet()
    {
        Items = _items;
        
        // Apply simple filtering based on query parameters
        if (!string.IsNullOrEmpty(Search))
        {
            Items = Items.Where(i => i.Name.Contains(Search, StringComparison.OrdinalIgnoreCase) ||
                                   i.Description.Contains(Search, StringComparison.OrdinalIgnoreCase))
                        .ToList();
        }
    }
}

public record Item(int Id, string Name, string Description);