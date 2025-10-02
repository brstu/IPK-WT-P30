using Microsoft.AspNetCore.Mvc;
using Task04.Web.Models;

namespace Task04.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            // Фиктивная авторизация
            if (model.Username == "admin" && model.Password == "password")
            {
                HttpContext.Session.SetString("IsAuthenticated", "true");
                HttpContext.Session.SetString("Username", model.Username);
                return RedirectToAction("Secret");
            }

            ViewBag.Error = "Неверные логин или пароль";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Secret()
        {
            // Проверка авторизации
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                return RedirectToAction("Login");
            }

            return View();
        }
    }
}