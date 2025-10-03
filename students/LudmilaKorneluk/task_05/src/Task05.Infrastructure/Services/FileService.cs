using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Task05.Application.Common.Interfaces;
using Task05.Domain.Entities;
using Task05.Infrastructure.Data;

namespace Task05.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly ApplicationDbContext _context;
    private readonly IHostEnvironment _environment;

    public FileService(ApplicationDbContext context, IHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<bool> UploadFileAsync(Stream fileStream, string fileName, string contentType, long fileSize, string uploadedBy)
    {
        try
        {
            // ... существующий код загрузки ...
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<Stream?> DownloadFileAsync(Guid fileId)
    {
        var fileUpload = await _context.FileUploads.FindAsync(fileId);
        if (fileUpload == null) return null;

        var uploadsFolder = Path.Combine(_environment.ContentRootPath, "wwwroot", "uploads");
        var filePath = Path.Combine(uploadsFolder, fileUpload.StoredFileName);

        return File.Exists(filePath) ? new FileStream(filePath, FileMode.Open, FileAccess.Read) : null;
    }

    public async Task<List<FileItem>> GetUserFilesAsync(string userId)
    {
        return await _context.FileUploads
            .Where(f => f.UploadedBy == userId)
            .OrderByDescending(f => f.UploadedAt)
            .Select(f => new FileItem(f.Id, f.OriginalFileName, f.UploadedAt, f.FileSize))
            .ToListAsync();
    }
}