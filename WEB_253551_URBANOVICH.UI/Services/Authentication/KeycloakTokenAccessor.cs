using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using WEB_253551_URBANOVICH.UI.Models;

namespace WEB_253551_URBANOVICH.UI.Services.Authentication;

public class KeycloakTokenAccessor : ITokenAccessor
{
    private readonly KeycloakData _keycloak;
    private readonly KeycloakServiceClientData _serviceClient;
    private readonly HttpContext? _httpContext;
    private readonly HttpClient _httpClient;

    public KeycloakTokenAccessor(
        IOptions<KeycloakData> options,
        IOptions<KeycloakServiceClientData> serviceClientOptions,
        IHttpContextAccessor httpContextAccessor,
        HttpClient httpClient)
    {
        _keycloak = options.Value;
        _serviceClient = serviceClientOptions.Value;
        _httpContext = httpContextAccessor.HttpContext;
        _httpClient = httpClient;
    }

    public async Task<string> GetAccessTokenAsync(CancellationToken ct = default)
    {
        // If user authenticated in UI, use their access_token from OIDC
        if (_httpContext?.User?.Identity?.IsAuthenticated == true)
        {
            var token = await _httpContext.GetTokenAsync("access_token");
            if (!string.IsNullOrWhiteSpace(token))
                return token;

            // if for some reason there is no user token, fall back to service account token
        }

        // Otherwise use client_credentials of service client (urbanovich-service)
        var tokenEndpoint =
            $"{_keycloak.Host}/realms/{_keycloak.Realm}/protocol/openid-connect/token";

        HttpContent content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("client_id", _serviceClient.ClientId),
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_secret", _serviceClient.ClientSecret),
        });

        var response = await _httpClient.PostAsync(tokenEndpoint, content, ct);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(response.StatusCode.ToString());

        var json = await response.Content.ReadAsStringAsync();
        var tokenNode = JsonNode.Parse(json)?["access_token"]?.GetValue<string>();

        if (string.IsNullOrWhiteSpace(tokenNode))
            throw new InvalidOperationException("Keycloak did not return access_token.");

        return tokenNode;
    }

    public async Task SetAuthorizationHeaderAsync(HttpClient httpClient, CancellationToken ct = default)
    {
        var token = await GetAccessTokenAsync(ct);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
