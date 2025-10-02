using Microsoft.AspNetCore.Http;
using Task05.Application.Interfaces;

namespace Task05.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly string _uploadPath;
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx" };
    private const long _maxFileSize = 2 * 1024 * 1024; // 2 МБ

    public FileService(IWebHostEnvironment environment)
    {
        _uploadPath = Path.Combine(environment.WebRootPath, "uploads");
        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }

    public bool ValidateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        if (file.Length > _maxFileSize)
            return false;

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return _allowedExtensions.Contains(extension);
    }

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        if (!ValidateFile(file))
            throw new InvalidOperationException("Файл не прошел валидацию");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var safeFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(_uploadPath, safeFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return safeFileName;
    }

    public async Task<(byte[] content, string fileName, string contentType)> GetFileAsync(string fileName)
    {
        var filePath = Path.Combine(_uploadPath, fileName);
        
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Файл не найден");

        var content = await File.ReadAllBytesAsync(filePath);
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        var contentType = GetContentType(extension);
        
        return (content, fileName, contentType);
    }

    private string GetContentType(string extension)
    {
        return extension switch
        {
            ".pdf" => "application/pdf",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            _ => "application/octet-stream"
        };
    }
}
