using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Task04.Web.Pages.Items
{
    public class IndexModel : PageModel
    {
        public List<Item> Items { get; set; } = new();

        public void OnGet()
        {
            // Тестовые данные
            Items = new List<Item>
            {
                new Item { Id = 1, Name = "Ноутбук", Price = 50000 },
                new Item { Id = 2, Name = "Смартфон", Price = 25000 },
                new Item { Id = 3, Name = "Планшет", Price = 15000 }
            };
        }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}