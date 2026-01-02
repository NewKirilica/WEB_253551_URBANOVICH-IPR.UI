using Microsoft.AspNetCore.Http;

namespace WEB_253551_URBANOVICH.UI.Services.FileService;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile formFile);

    Task DeleteFileAsync(string fileName);
}
