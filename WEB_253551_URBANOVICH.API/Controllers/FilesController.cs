using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WEB_253551_URBANOVICH.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "admin")]
public class FilesController : ControllerBase
{
    private readonly string _imagePath;

    public FilesController(IWebHostEnvironment webHost)
    {
        _imagePath = Path.Combine(webHost.WebRootPath, "Images");
        Directory.CreateDirectory(_imagePath);
    }

    [HttpPost]
    public async Task<IActionResult> SaveFile(IFormFile file)
    {
        if (file is null)
            return BadRequest();

        var filePath = Path.Combine(_imagePath, file.FileName);
        var fileInfo = new FileInfo(filePath);

        if (fileInfo.Exists)
            fileInfo.Delete();

        using var fileStream = fileInfo.Create();
        await file.CopyToAsync(fileStream);

        var host = HttpContext.Request.Host;
        var scheme = HttpContext.Request.Scheme;
        var fileUrl = $"{scheme}://{host}/Images/{file.FileName}";
        return Ok(fileUrl);
    }

    [HttpDelete]
    public IActionResult DeleteFile([FromQuery] string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return BadRequest();

        var safeName = Path.GetFileName(fileName);
        var filePath = Path.Combine(_imagePath, safeName);
        var fileInfo = new FileInfo(filePath);
        if (fileInfo.Exists)
            fileInfo.Delete();

        return Ok();
    }
}
