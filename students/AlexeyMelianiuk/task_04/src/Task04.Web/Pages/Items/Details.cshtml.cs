using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task04.Web.Pages.Items;

public class DetailsModel : PageModel
{
    private static readonly List<Item> _items = new()
    {
        new Item(1, "Элемент 1", "Подробное описание первого элемента"),
        new Item(2, "Элемент 2", "Подробное описание второго элемента"),
        new Item(3, "Элемент 3", "Подробное описание третьего элемента")
    };

    public Item? Item { get; private set; }

    public IActionResult OnGet(int id)
    {
        Item = _items.FirstOrDefault(i => i.Id == id);
        
        if (Item == null)
        {
            return NotFound();
        }

        return Page();
    }
}