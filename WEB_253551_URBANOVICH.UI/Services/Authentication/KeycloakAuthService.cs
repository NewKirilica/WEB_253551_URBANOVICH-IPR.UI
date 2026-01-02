using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using WEB_253551_URBANOVICH.UI.Models;
using WEB_253551_URBANOVICH.UI.Services.FileService;

namespace WEB_253551_URBANOVICH.UI.Services.Authentication;

public class KeycloakAuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IFileService _fileService;
    private readonly ITokenAccessor _tokenAccessor;
    private readonly KeycloakData _keycloakData;

    // Admin API payload models (Keycloak expects attributes as arrays of strings)
    private sealed class CreateUserModel
    {
        public Dictionary<string, string[]> Attributes { get; set; } = new();
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Enabled { get; set; } = true;
        public bool EmailVerified { get; set; } = true;
        public List<UserCredentials> Credentials { get; set; } = new();
    }

    private sealed class UserCredentials
    {
        public string Type { get; set; } = "password";
        public bool Temporary { get; set; } = false;
        public string Value { get; set; } = string.Empty;
    }

    public KeycloakAuthService(
        HttpClient httpClient,
        IOptions<KeycloakData> options,
        IFileService fileService,
        ITokenAccessor tokenAccessor)
    {
        _httpClient = httpClient;
        _fileService = fileService;
        _tokenAccessor = tokenAccessor;
        _keycloakData = options.Value;
    }

    public async Task<(bool Result, string ErrorMessage)> RegisterUserAsync(
        string email,
        string password,
        IFormFile? avatar,
        CancellationToken ct = default)
    {
        try
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient, ct);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }

        // Default avatar (must exist in API wwwroot/Images)
        var avatarUrl = "/Images/default-profile-picture.png";

        // Save avatar in API (wwwroot/Images) if it was provided
        if (avatar != null)
        {
            var savedUrl = await _fileService.SaveFileAsync(avatar);
            if (!string.IsNullOrWhiteSpace(savedUrl))
                avatarUrl = savedUrl;
        }

        var newUser = new CreateUserModel
        {
            Email = email,
            Username = email,
            Credentials = new List<UserCredentials>
            {
                new() { Value = password }
            }
        };

        newUser.Attributes["avatar"] = new[] { avatarUrl };

        var requestUri = $"{_keycloakData.Host}/admin/realms/{_keycloakData.Realm}/users";

        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var userData = JsonSerializer.Serialize(newUser, serializerOptions);
        using HttpContent content = new StringContent(userData, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(requestUri, content, ct);
        if (response.IsSuccessStatusCode)
            return (true, string.Empty);

        return (false, response.StatusCode.ToString());
    }
}
