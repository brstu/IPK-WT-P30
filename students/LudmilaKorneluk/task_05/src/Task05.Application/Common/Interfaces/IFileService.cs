namespace Task05.Application.Common.Interfaces;

public interface IFileService
{
    Task<bool> UploadFileAsync(Stream fileStream, string fileName, string contentType, long fileSize, string uploadedBy);
    Task<Stream?> DownloadFileAsync(Guid fileId);
    Task<List<FileItem>> GetUserFilesAsync(string userId);
}

public record FileItem(Guid Id, string OriginalFileName, DateTime UploadedAt, long FileSize);