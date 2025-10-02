using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace Task05.Web.Pages
{
    public class FileDownloadModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;

        public FileDownloadModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult OnGet(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return NotFound();
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            // Определяем Content-Type
            var contentType = GetContentType(fileName);
            var safeFileName = Path.GetFileName(filePath);

            // Возвращаем файл с правильными заголовками
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, contentType, safeFileName);
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".txt" => "text/plain",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream"
            };
        }
    }
}