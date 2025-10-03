using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ElectronicStore.Models;

namespace ElectronicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "iPhone 15", Category = "Смартфоны", Price = 999.99m, Description = "Новейший смартфон от Apple", Stock = 50 },
            new Product { Id = 2, Name = "Samsung Galaxy S24", Category = "Смартфоны", Price = 899.99m, Description = "Флагманский смартфон от Samsung", Stock = 30 },
            new Product { Id = 3, Name = "MacBook Pro", Category = "Ноутбуки", Price = 1999.99m, Description = "Мощный ноутбук для профессионалов", Stock = 20 },
            new Product { Id = 4, Name = "Sony WH-1000XM5", Category = "Наушники", Price = 349.99m, Description = "Беспроводные наушники с шумоподавлением", Stock = 100 }
        };

        public IActionResult Index()
        {
            ViewData["Title"] = "Главная - Магазин электроники";
            ViewData["IsAuthenticated"] = HttpContext.Session.GetString("IsAuthenticated") == "true";
            return View(_products.Take(3).ToList());
        }

        public IActionResult List()
        {
            ViewData["Title"] = "Список товаров";
            ViewData["IsAuthenticated"] = HttpContext.Session.GetString("IsAuthenticated") == "true";
            return View(_products);
        }

        [Route("Home/Details/{id:int}")]
        public IActionResult Details(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            ViewData["Title"] = $"Детали - {product.Name}";
            ViewData["IsAuthenticated"] = HttpContext.Session.GetString("IsAuthenticated") == "true";
            return View(product);
        }

        public IActionResult Login()
        {
            ViewData["Title"] = "Вход в систему";
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (model.Username == "admin" && model.Password == "password")
            {
                HttpContext.Session.SetString("IsAuthenticated", "true");
                HttpContext.Session.SetString("Username", model.Username);
                return RedirectToAction("Index");
            }

            ViewData["ErrorMessage"] = "Неверное имя пользователя или пароль";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("IsAuthenticated");
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Index");
        }

        [CustomAuth]
        public IActionResult Orders()
        {
            ViewData["Title"] = "Мои заказы";
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            return View();
        }

        [HttpPost]
        [CustomAuth]
        public IActionResult CreateOrder(OrderModel order)
        {
            ViewData["Title"] = "Заказ создан";
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            return View(order);
        }
    }

    public class CustomAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isAuthenticated = context.HttpContext.Session.GetString("IsAuthenticated");
            if (isAuthenticated != "true")
            {
                context.Result = new RedirectToActionResult("Login", "Home", null);
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}