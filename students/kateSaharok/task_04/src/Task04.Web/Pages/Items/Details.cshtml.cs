using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Task04.Web.Pages.Items
{
    public class DetailsModel : PageModel
    {
        public ItemDetails Item { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            // Тестовые данные
            var items = new List<ItemDetails>
            {
                new ItemDetails { Id = 1, Name = "Ноутбук", Price = 50000, Description = "Мощный игровой ноутбук" },
                new ItemDetails { Id = 2, Name = "Смартфон", Price = 25000, Description = "Флагманский смартфон" },
                new ItemDetails { Id = 3, Name = "Планшет", Price = 15000, Description = "Планшет для работы и развлечений" }
            };

            var item = items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            Item = item;
            return Page();
        }
    }

    public class ItemDetails
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}