using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task05.Application.Interfaces;

namespace Task05.Web.Controllers;

[Authorize]
public class FileController : Controller
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpGet]
    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    [RequestSizeLimit(2 * 1024 * 1024)] // Ограничение 2 МБ
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("", "Файл не выбран");
            return View();
        }

        try
        {
            var savedFileName = await _fileService.SaveFileAsync(file);
            ViewBag.Message = $"Файл успешно загружен: {savedFileName}";
            ViewBag.Success = true;
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Download(string fileName)
    {
        try
        {
            var (content, originalFileName, contentType) = await _fileService.GetFileAsync(fileName);
            
            // Установка правильных заголовков для скачивания [citation:4][citation:9]
            Response.Headers.Append("Content-Disposition", $"attachment; filename=\"{originalFileName}\"");
            return File(content, contentType, originalFileName);
        }
        catch (FileNotFoundException)
        {
            return NotFound();
        }
    }
}
