using System.Text.Json;
using System.Net.Http.Json;
using WEB_253551_URBANOVICH.Domain.Entities;
using WEB_253551_URBANOVICH.Domain.Models;

namespace WEB_253551_URBANOVICH.UI.Services.CategoryService;

public class ApiCategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;

    public ApiCategoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + "Categories");
        var response = await _httpClient.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
            return ResponseData<List<Category>>.Error($"Данные не получены. Error: {response.StatusCode}");

        try
        {
            var data = await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions);
            return data ?? ResponseData<List<Category>>.Error("Пустой ответ сервера");
        }
        catch (JsonException ex)
        {
            return ResponseData<List<Category>>.Error($"Ошибка: {ex.Message}");
        }
    }
}
