using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task04.Web.Pages.Items
{
    public class IndexModel : PageModel
    {
        public List<Item> Items { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public int Count { get; set; } = 5;

        public void OnGet()
        {
            // Генерация тестовых данных
            Items = Enumerable.Range(1, 10)
                .Select(i => new Item { Id = i, Name = $"Элемент {i}" })
                .ToList();

            // Фильтрация по поиску
            if (!string.IsNullOrEmpty(Search))
            {
                Items = Items.Where(i => i.Name.Contains(Search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Ограничение количества
            if (Count > 0)
            {
                Items = Items.Take(Count).ToList();
            }
        }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}