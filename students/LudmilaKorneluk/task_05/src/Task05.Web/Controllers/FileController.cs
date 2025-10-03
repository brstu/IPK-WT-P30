using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task05.Application.Common.Interfaces;

namespace Task05.Web.Controllers;

[Authorize]
public class FileController : Controller
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task<IActionResult> Index()
    {
        var files = await _fileService.GetUserFilesAsync(User.Identity!.Name!);
        return View(files);
    }

    [HttpPost]
    [RequestSizeLimit(2 * 1024 * 1024)] // 2MB limit
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            TempData["Error"] = "Please select a file to upload.";
            return RedirectToAction(nameof(Index));
        }

        var result = await _fileService.UploadFileAsync(
            file.OpenReadStream(),
            file.FileName,
            file.ContentType,
            file.Length,
            User.Identity!.Name!);

        if (result.Success)
        {
            TempData["Success"] = "File uploaded successfully.";
        }
        else
        {
            TempData["Error"] = result.ErrorMessage;
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Download(Guid id)
    {
        var result = await _fileService.DownloadFileAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        return File(result.FileStream, result.ContentType, result.FileName);
    }
}