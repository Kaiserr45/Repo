using System.Net.Http.Headers;

namespace IngoX.Client.Bff.Handlers;

/// <summary>
/// Проброс заголовка Authorization.
/// </summary>
public class AuthTokenHandler : HttpClientHandler
{
    private const string AuthTokenHeaderName = "Authorization";
    private IHttpContextAccessor httpContext;

    public AuthTokenHandler(IServiceProvider serviceProvider)
    {
        httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = httpContext.HttpContext.Request.Headers[AuthTokenHeaderName];
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }

    /// <inheritdoc/>
    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = httpContext.HttpContext.Request.Headers[AuthTokenHeaderName];
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return base.Send(request, cancellationToken);
    }
}
