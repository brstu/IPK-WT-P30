using Task05.Domain.Models;

namespace Task05.Application.Interfaces;

public interface IFileService
{
    Task<FileUpload?> UploadFileAsync(Stream fileStream, string fileName, string contentType, string userId);
    Task<FileUpload?> GetFileAsync(Guid fileId);
    Task<bool> DeleteFileAsync(Guid fileId);
    Task<IEnumerable<FileUpload>> GetUserFilesAsync(string userId);
}