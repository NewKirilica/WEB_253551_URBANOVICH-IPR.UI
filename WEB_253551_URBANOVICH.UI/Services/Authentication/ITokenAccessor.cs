namespace WEB_253551_URBANOVICH.UI.Services.Authentication;

public interface ITokenAccessor
{
    Task<string> GetAccessTokenAsync(CancellationToken ct = default);
    Task SetAuthorizationHeaderAsync(HttpClient httpClient, CancellationToken ct = default);
}
