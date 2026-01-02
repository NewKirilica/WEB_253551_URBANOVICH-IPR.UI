using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using WEB_253551_URBANOVICH.Domain.Entities;
using WEB_253551_URBANOVICH.Domain.Models;
using WEB_253551_URBANOVICH.UI.Services.FileService;
using WEB_253551_URBANOVICH.UI.Services.Authentication;

namespace WEB_253551_URBANOVICH.UI.Services.ProductService;

public class ApiProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly IFileService _fileService;
    private readonly ITokenAccessor _tokenAccessor;
    private readonly ILogger<ApiProductService> _logger;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly string _pageSize;

    public ApiProductService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ApiProductService> logger,
        IFileService fileService,
        ITokenAccessor tokenAccessor)
    {
        _httpClient = httpClient;
        _logger = logger;
        _fileService = fileService;
        _tokenAccessor = tokenAccessor;

        _pageSize = configuration.GetSection("ItemsPerPage").Value ?? "3";
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
        if (pageNo < 1) pageNo = 1;

        var query = new Dictionary<string, string?>
        {
            ["pageNo"] = pageNo.ToString(),
            ["pageSize"] = _pageSize
        };

        if (!string.IsNullOrWhiteSpace(categoryNormalizedName))
            query["category"] = categoryNormalizedName;

        var url = QueryHelpers.AddQueryString("Dishes", query);

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode}");
            return ResponseData<ListModel<Dish>>.Error($"Данные не получены от сервера. Error: {response.StatusCode}");
        }

        try
        {
            var data = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Dish>>>(_serializerOptions);
            return data ?? ResponseData<ListModel<Dish>>.Error("Пустой ответ сервера");
        }
        catch (JsonException ex)
        {
            _logger.LogError($"-----> Ошибка: {ex.Message}");
            return ResponseData<ListModel<Dish>>.Error($"Ошибка: {ex.Message}");
        }
    }

    public async Task<ResponseData<Dish>> GetProductByIdAsync(int id)
    {
        var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + $"Dishes/byid/{id}");
        var response = await _httpClient.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
            return ResponseData<Dish>.Error($"Данные не получены. Error: {response.StatusCode}");

        try
        {
            var data = await response.Content.ReadFromJsonAsync<ResponseData<Dish>>(_serializerOptions);
            return data ?? ResponseData<Dish>.Error("Пустой ответ сервера");
        }
        catch (JsonException ex)
        {
            return ResponseData<Dish>.Error($"Ошибка: {ex.Message}");
        }
    }

    public async Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        product.Image = "Images/noimage.jpg";

        if (formFile != null)
        {
            var imageUrl = await _fileService.SaveFileAsync(formFile);
            if (!string.IsNullOrEmpty(imageUrl))
                product.Image = imageUrl;
        }

        product.Category = null!;

        var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + "Dishes");
        var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<ResponseData<Dish>>(_serializerOptions);
            return data ?? ResponseData<Dish>.Error("Пустой ответ сервера");
        }

        _logger.LogError($"-----> object not created. Error: {response.StatusCode}");
        return ResponseData<Dish>.Error($"Объект не добавлен. Error: {response.StatusCode}");
    }

    public async Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish product, IFormFile? formFile)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        if (formFile != null)
        {
            var old = product.Image;
            var oldFile = ExtractFileName(old);
            if (!string.IsNullOrEmpty(oldFile) && !oldFile.Equals("noimage.jpg", StringComparison.OrdinalIgnoreCase))
                await _fileService.DeleteFileAsync(oldFile);

            var imageUrl = await _fileService.SaveFileAsync(formFile);
            if (!string.IsNullOrEmpty(imageUrl))
                product.Image = imageUrl;
        }

        product.Category = null!;

        var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + $"Dishes/{id}");
        var response = await _httpClient.PutAsJsonAsync(uri, product, _serializerOptions);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<ResponseData<Dish>>(_serializerOptions);
            return data ?? ResponseData<Dish>.Error("Пустой ответ сервера");
        }

        _logger.LogError($"-----> object not updated. Error: {response.StatusCode}");
        return ResponseData<Dish>.Error($"Объект не изменен. Error: {response.StatusCode}");
    }

    public async Task DeleteProductAsync(int id)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + $"Dishes/{id}");
        await _httpClient.DeleteAsync(uri);
    }

    private static string ExtractFileName(string? image)
    {
        if (string.IsNullOrWhiteSpace(image))
            return string.Empty;

        try
        {
            if (Uri.TryCreate(image, UriKind.Absolute, out var uri))
                return Path.GetFileName(uri.LocalPath);

            return Path.GetFileName(image);
        }
        catch
        {
            return string.Empty;
        }
    }
}
