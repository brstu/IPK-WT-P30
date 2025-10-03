using Microsoft.AspNetCore.Mvc;
using Task04.Web.Models;

namespace Task04.Web.Controllers
{
    public class ItemsController : Controller
    {
        private static List<Item> _items = new()
        {
            new Item { Id = 1, Name = "Ноутбук", Description = "Игровой ноутбук", Price = 1500 },
            new Item { Id = 2, Name = "Мышь", Description = "Беспроводная мышь", Price = 50 },
            new Item { Id = 3, Name = "Клавиатура", Description = "Механическая клавиатура", Price = 120 },
            new Item { Id = 4, Name = "Монитор", Description = "27-дюймовый монитор", Price = 300 }
        };

        public IActionResult Index()
        {
            return View(_items);
        }

        [Route("Items/Details/{id:int}")]
        public IActionResult Details(int id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // Демонстрация привязки модели из разных источников
        [HttpGet]
        public IActionResult Search([FromQuery] string name, [FromQuery] decimal? minPrice)
        {
            var results = _items.AsEnumerable();
            
            if (!string.IsNullOrEmpty(name))
                results = results.Where(i => i.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            
            if (minPrice.HasValue)
                results = results.Where(i => i.Price >= minPrice.Value);

            ViewBag.SearchName = name;
            ViewBag.SearchMinPrice = minPrice;
            
            return View(results.ToList());
        }

        [HttpPost]
        public IActionResult AddItem([FromBody] Item item)
        {
            if (item != null)
            {
                item.Id = _items.Max(i => i.Id) + 1;
                _items.Add(item);
                return Json(new { success = true, id = item.Id });
            }
            return Json(new { success = false });
        }
    }
}