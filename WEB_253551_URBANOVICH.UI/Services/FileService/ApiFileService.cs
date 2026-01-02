using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using WEB_253551_URBANOVICH.UI.Services.Authentication;

namespace WEB_253551_URBANOVICH.UI.Services.FileService;

public class ApiFileService : IFileService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenAccessor _tokenAccessor;

    public ApiFileService(HttpClient httpClient, ITokenAccessor tokenAccessor)
    {
        _httpClient = httpClient;
        _tokenAccessor = tokenAccessor;
    }

    public async Task<string> SaveFileAsync(IFormFile formFile)
    {
        // ВАЖНО: добавляем Bearer-токен (будет client_credentials от urbanovich-service, если пользователь не залогинен)
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

        var extension = Path.GetExtension(formFile.FileName);
        var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);

        using var content = new MultipartFormDataContent();
        using var streamContent = new StreamContent(formFile.OpenReadStream());

        streamContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);
        content.Add(streamContent, "file", newName); // имя поля "file" должно совпадать с API

        var response = await _httpClient.PostAsync(_httpClient.BaseAddress, content);
        if (!response.IsSuccessStatusCode)
            return string.Empty;

        return (await response.Content.ReadAsStringAsync()).Trim('"');
    }

    public async Task DeleteFileAsync(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return;

        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

        var safeName = Path.GetFileName(fileName);
        var uri = new Uri($"{_httpClient.BaseAddress}?fileName={Uri.EscapeDataString(safeName)}");
        await _httpClient.DeleteAsync(uri);
    }
}
