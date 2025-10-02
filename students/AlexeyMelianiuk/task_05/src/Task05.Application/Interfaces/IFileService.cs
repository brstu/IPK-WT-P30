using Microsoft.AspNetCore.Http;

namespace Task05.Application.Interfaces;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file);
    Task<(byte[] content, string fileName, string contentType)> GetFileAsync(string fileName);
    bool ValidateFile(IFormFile file);
}