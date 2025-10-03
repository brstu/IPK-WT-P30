using Microsoft.AspNetCore.Mvc;

namespace ElectronicStore.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Ping()
        {
            return Content("Pong! Server is working.");
        }
        
        public IActionResult SessionTest()
        {
            HttpContext.Session.SetString("Test", "Session works!");
            var testValue = HttpContext.Session.GetString("Test");
            return Content($"Session test: {testValue}");
        }
    }
}
