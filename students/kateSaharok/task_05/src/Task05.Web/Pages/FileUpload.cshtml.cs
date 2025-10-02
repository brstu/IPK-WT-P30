using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace Task05.Web.Pages
{
    public class FileUploadModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private const long MaxFileSize = 2 * 1024 * 1024; // 2 МБ
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf", ".txt", ".doc", ".docx" };

        [BindProperty]
        public IFormFile? UploadedFile { get; set; }

        public string Message { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public List<FileItem> UploadedFiles { get; set; } = new();

        public FileUploadModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void OnGet()
        {
            LoadUploadedFiles();
        }

        public async Task<IActionResult> OnPost()
        {
            if (UploadedFile == null || UploadedFile.Length == 0)
            {
                ErrorMessage = "Файл не выбран";
                LoadUploadedFiles();
                return Page();
            }

            // Проверка размера
            if (UploadedFile.Length > MaxFileSize)
            {
                ErrorMessage = $"Файл слишком большой. Максимальный размер: {MaxFileSize / 1024 / 1024} МБ";
                LoadUploadedFiles();
                return Page();
            }

            // Проверка расширения
            var fileExtension = Path.GetExtension(UploadedFile.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(fileExtension) || !AllowedExtensions.Contains(fileExtension))
            {
                ErrorMessage = $"Недопустимый тип файла. Разрешенные типы: {string.Join(", ", AllowedExtensions)}";
                LoadUploadedFiles();
                return Page();
            }

            try
            {
                // Создаем безопасное имя файла
                var safeFileName = $"{Guid.NewGuid()}{fileExtension}";
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                
                // Создаем папку если её нет
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, safeFileName);

                // Сохраняем файл
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadedFile.CopyToAsync(stream);
                }

                Message = $"Файл '{UploadedFile.FileName}' успешно загружен!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при загрузке файла: {ex.Message}";
            }

            LoadUploadedFiles();
            return Page();
        }

        private void LoadUploadedFiles()
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                return;

            var fileSystemInfos = Directory.GetFiles(uploadsFolder)
                .Select(filePath => new System.IO.FileInfo(filePath))
                .OrderByDescending(f => f.CreationTime)
                .Take(10)
                .Select(f => new FileItem
                {
                    SafeName = f.Name,
                    OriginalName = f.Name,
                    SizeInKB = (int)(f.Length / 1024),
                    UploadDate = f.CreationTime
                })
                .ToList();

            UploadedFiles = fileSystemInfos;
        }
    }

    public class FileItem
    {
        public string SafeName { get; set; } = string.Empty;
        public string OriginalName { get; set; } = string.Empty;
        public int SizeInKB { get; set; }
        public DateTime UploadDate { get; set; }
    }
}