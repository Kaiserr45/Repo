using Microsoft.Extensions.Configuration;

namespace IngoX.Client.Bff.Config;

/// <summary>
/// Расширения для <see cref="IConfiguration"/>.
/// </summary>
public static class ConfigurationExtension
{
    /// <summary>
    /// Получает короткий путь префикса.
    /// </summary>
    /// <param name="configuration"><see cref="IConfiguration"/>.</param>
    public static string GetShortRoutePrefix(this IConfiguration configuration)
    {
        var pathPrefix = configuration.GetValue<string>("IngosApplication:PathPrefix");
        var serviceType = configuration.GetValue<string>("IngosApplication:ServiceType");
        var serviceAlias = configuration.GetValue<string>("IngosApplication:ServiceAlias");
        var apiVersion = configuration.GetValue<string>("IngosApplication:ApiVersion");
        var serviceSuffix = !string.IsNullOrWhiteSpace(serviceAlias) ? serviceAlias : serviceType;

        return $"{pathPrefix}/{serviceSuffix}/{apiVersion}";
    }
}
