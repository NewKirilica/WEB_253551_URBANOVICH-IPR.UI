using Serilog;

namespace WEB_253551_URBANOVICH.UI.Middleware;

public class LogMiddleware
{
    private readonly RequestDelegate _next;

    public LogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        await _next(httpContext);

        var code = httpContext.Response.StatusCode;
        if (code < 200 || code >= 300)
        {
            var path = httpContext.Request.Path.Value ?? "/";
            Log.Information("---> request {Path} returns {StatusCode}", path, code);
        }
    }
}
