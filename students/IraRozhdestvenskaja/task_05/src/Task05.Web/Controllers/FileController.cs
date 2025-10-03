using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Task05.Application.Interfaces;
using System.Security.Claims;

namespace Task05.Web.Controllers;

[Authorize]
public class FileController : Controller
{
    private readonly IFileService _fileService;
    private readonly IWebHostEnvironment _environment;

    public FileController(IFileService fileService, IWebHostEnvironment environment)
    {
        _fileService = fileService;
        _environment = environment;
    }

    [HttpGet]
    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    [RequestSizeLimit(10 * 1024 * 1024)]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("", "Please select a file");
            return View();
        }

        // File type validation
        var allowedExtensions = new[] { ".txt", ".pdf", ".doc", ".docx", ".jpg", ".png" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
        {
            ModelState.AddModelError("", "Invalid file type. Allowed: .txt, .pdf, .doc, .docx, .jpg, .png");
            return View();
        }

        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";
            var result = await _fileService.UploadFileAsync(
                file.OpenReadStream(), 
                file.FileName, 
                file.ContentType, 
                userId);

            if (result != null)
            {
                ViewBag.Message = $"File '{file.FileName}' uploaded successfully!";
            }
            else
            {
                ModelState.AddModelError("", "Error uploading file");
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error: {ex.Message}");
        }

        return View();
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Admin()
    {
        return View();
    }
}