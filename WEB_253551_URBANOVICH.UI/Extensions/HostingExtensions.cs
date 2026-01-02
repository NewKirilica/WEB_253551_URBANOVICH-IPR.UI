using WEB_253551_URBANOVICH.UI.Models;
using WEB_253551_URBANOVICH.UI.Services.Authentication;
using WEB_253551_URBANOVICH.UI.Services.CategoryService;
using WEB_253551_URBANOVICH.UI.Services.FileService;
using WEB_253551_URBANOVICH.UI.Services.ProductService;

namespace WEB_253551_URBANOVICH.UI.Extensions;

public static class HostingExtensions
{
    public static void RegisterCustomServices(this WebApplicationBuilder builder)
    {
        var uriData = new UriData();
        builder.Configuration.GetSection("UriData").Bind(uriData);
        builder.Services.AddSingleton(uriData);

        // Keycloak settings used by OIDC, token accessor and registration service
        builder.Services.Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));
        builder.Services.Configure<KeycloakServiceClientData>(builder.Configuration.GetSection("KeycloakServiceClient"));

        builder.Services.AddHttpContextAccessor();

        // Token accessor (uses user token if authenticated, otherwise client_credentials)
        builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();

        // Keycloak admin API user registration (creates users via Admin REST API)
        builder.Services.AddHttpClient<IAuthService, KeycloakAuthService>();

        builder.Services
            .AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
                opt.BaseAddress = new Uri(uriData.ApiUri));

        builder.Services
            .AddHttpClient<IProductService, ApiProductService>(opt =>
                opt.BaseAddress = new Uri(uriData.ApiUri));

        builder.Services
            .AddHttpClient<IFileService, ApiFileService>(opt =>
                opt.BaseAddress = new Uri(new Uri(uriData.ApiUri), "Files"));

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession();

    }
}
