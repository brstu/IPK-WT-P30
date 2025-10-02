using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Task05.Web.Controllers;

[Authorize(Roles = "Admin")]
public class FilesController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Upload(IFormFile file) // Убрали async, т.к. не используем await
    {
        if (file == null || file.Length == 0)
        {
            ViewBag.Message = "Файл не выбран";
            return View("Index");
        }
        
        // Здесь будет логика сохранения файла
        ViewBag.Message = $"Файл '{file.FileName}' успешно загружен!";
        return View("Index");
    }
}