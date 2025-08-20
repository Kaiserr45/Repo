using Ingos.Core.Services;
using Ingos.Platform.Abstract.App;
using Ingos.Platform.Abstract.Process;
using Ingos.Platform.Authorization.Extensions;
using Ingos.Platform.Authorization.Models;
using Ingos.Platform.Authorization.Swagger.Extensions;
using Ingos.Platform.Core.Services;
using Ingos.Platform.DependencyInspector.Extensions;
using Ingos.Platform.Errors.Extensions;
using Ingos.Platform.HealthCheck.Extensions;
using Ingos.Platform.Logging.Serilog.Extentions;
using Ingos.Platform.Logging.Serilog.Helpers;
using Ingos.Platform.Mapper.Extensions;
using Ingos.Platform.ReCaptcha.Extensions;
using Ingos.Platform.WebApi.Common.Extensions;
using IngoX.Client.Bff.Core.Abstract;
using IngoX.Client.Bff.Core.Mappers;
using IngoX.Client.Bff.Core.Models.External;
using IngoX.Client.Bff.Core.Models.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace IngoX.Client.Bff.Config;

/// <summary>
///  Настройка IoC.
/// </summary>
public static class ConfigureModuleServices
{
    /// <summary>
    /// Конфигурация сервисов IoC.
    /// </summary>
    /// <param name="services"><inheritdoc cref="IServiceCollection" path="/summary"/></param>
    /// <returns></returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // Провайдер для получения информации о времени работы приложения
        services.AddSingleton<IAppUptimeProvider, AppUptimeProvider>();

        // Доступ к сервису общей информации по приложению
        services.AddSingleton<IAppInfoProvider, AppInfoProvider>();

        // Доступ к контексту вызова
        services.AddSingleton<ISourceContextProvider, SourceContextProvider>();

        return services;
    }

    /// <summary>
    /// Добавляет регистрацию сервисов Ингостраха.
    /// </summary>
    /// <param name="services"><inheritdoc cref="IServiceCollection" path="/summary"/></param>
    /// <param name="configuration"><inheritdoc cref="IConfiguration" path="/summary"/></param>
    public static IServiceCollection AddIngosServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IAppUptimeProvider, AppUptimeProvider>();
        services.AddSingleton<IAppInfoProvider, AppInfoProvider>();
        services.AddSingleton<ISourceContextProvider, SourceContextProvider>();

        // Обработчик ошибок валидации
        services.AddIngosErrors(configuration);

        // Регистрация сервиса мониторинга
        services.AddIngosHealthChecks(configuration);

        services.AddIngosCaptcha(configuration);

        services.AddIngosLogging(
                    configuration,
                    configureEnricher: ctx => ctx
                        .AddDefaultWebRequestCookiePolicy()
                        .AddDefaultWebResponseCookiePolicy());

        // services.AddIngosAuthorization(configuration);
        services.AddIngosAuthorization(configuration, options =>
        {
            options.Swagger.Shemes.Add(IngosSchemeDefaults.ClientScheme, new()
            {
                Type = SecuritySchemeType.OAuth2,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Flows = new OpenApiOAuthFlows()
                {
                    AuthorizationCode = OpenApiOAuthFlowExtensions.CreateFlow(new Dictionary<string, string>()
                    {
                        {
                            "openid",
                            "Openid"
                        },
                        {
                            "app.ais.INGOGATE",
                            "ais app"
                        }
                    })
                },
                In = ParameterLocation.Header,
                Name = HeaderNames.Authorization
            });
        });

        // services.AddIngosAuthorizationClient(configuration);
        services.AddIngosDependencyInpector(configuration);
        services.AddIngosSwagger(
            configuration,
            spec => spec.PathPrefix = configuration.GetShortRoutePrefix(),
            c => c.AddCaptchaOperationFilters());

        services.AddIngosMapper();

        return services;
    }

    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDirectMapper<SearchPersonItem, Person>, SearchPersonItemToPersonMapper>();
        services.AddScoped<IDirectMapper<AdvancedSearchPersonItem, Person>, AdvancedSearchPersonItemToPersonMapper>();
        return services;
    }

    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}
