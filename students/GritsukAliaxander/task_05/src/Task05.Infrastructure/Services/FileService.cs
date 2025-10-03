using Task05.Application.Interfaces;
using Task05.Domain.Models;

namespace Task05.Infrastructure.Services;

public class FileService : IFileService
{
    public Task<FileUpload?> UploadFileAsync(Stream fileStream, string fileName, string contentType, string userId)
    {
        // Простая заглушка для тестирования
        var result = new FileUpload
        {
            Id = Guid.NewGuid(),
            OriginalFileName = fileName,
            StoredFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}",
            ContentType = contentType,
            FileSize = fileStream.Length,
            UploadDate = DateTime.UtcNow,
            UploadedBy = userId
        };

        return Task.FromResult<FileUpload?>(result);
    }

    public Task<FileUpload?> GetFileAsync(Guid fileId) => Task.FromResult<FileUpload?>(null);
    public Task<bool> DeleteFileAsync(Guid fileId) => Task.FromResult(true);
    public Task<IEnumerable<FileUpload>> GetUserFilesAsync(string userId) => Task.FromResult(Enumerable.Empty<FileUpload>());
}