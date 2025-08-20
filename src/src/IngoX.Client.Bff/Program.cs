using Ingos.Platform.Abstract.Models;
using Ingos.Platform.Authorization.Extensions;
using Ingos.Platform.Errors.Extensions;
using Ingos.Platform.HealthCheck.Extensions;
using Ingos.Platform.Logging.Serilog;
using Ingos.Platform.ReCaptcha.Extensions;
using Ingos.Platform.WebApi.Common.Extensions;
using Ingos.Platform.WebApi.Common.Helpers.Serialization;
using Ingos.Platform.WebApi.Common.Models;
using Ingos.Platform.WebApi.Extensions;
using IngoX.Client.Bff.Config;
using IngoX.Client.Bff.Core.Abstract;
using IngoX.Client.Bff.Core.Models.UI;
using IngoX.Client.Bff.Core.Services;
using IngoX.Client.Bff.Core.Services.Mock;
using IngoX.Client.Bff.Handlers;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;

#pragma warning disable SA1516
#pragma warning disable SA1515
#pragma warning disable SA10015

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Logging.SetMinimumLevel(LogLevel.Debug);
// builder.Configuration.AddVault();
builder.Services.AddOptions();
builder.Services.Configure<AppOptions>(builder.Configuration.GetSection("IngosApplication"));
builder.Services.Configure<ApiSpecOptions>(builder.Configuration.GetSection("IngosApiSpecification"));

var shortRoutePrefix = builder.Configuration.GetShortRoutePrefix();

builder.Services.AddCors(options => options.AddIngosCors(builder.Configuration));

builder.Services
    .AddControllers(options =>
    {
        options.UseCentralRoutePrefix(new RouteAttribute(shortRoutePrefix));
        options.AddIngosResponseCache();
    })
    .AddJsonOptions(jsonOptions =>
    {
        JsonSerializationConfig.ApplyOptions(jsonOptions);
    });

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownProxies.Clear();
    options.KnownNetworks.Clear();
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddMemoryCache();

builder.Services
    .AddApplicationServices()
    .AddIngosServices(builder.Configuration)
    .AddCoreServices(builder.Configuration)
    .AddDataServices(builder.Configuration);

builder.Services.AddRefitClient<IIndividualClientApp>()
  .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration.GetSection("IndividualClientApi")["BaseUrl"]))
  .ConfigurePrimaryHttpMessageHandler(x => new AuthTokenHandler(x));

builder.Services.AddScoped<IPersonSearchService, PersonSearchService>();
builder.Services.AddScoped<ILastUserQueriesService<Person>, LastUserQueriesServiceMock>();
builder.Services.AddScoped<INameSuggestionService, NameSuggestionServiceMock>();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseForwardedHeaders();
app.UseIngosLoggingMiddlewares();
app.UseIngosErrors();
app.UseRouting();
app.UseCors(WebApiOptionsExtensions.IngosDefaultCorsPolicy);
app.UseIngosAuthorization();

app.UseCaptchaServices();

app.UseEndpoints(endpoints =>
{
    endpoints.MapIngosHealthChecks(shortRoutePrefix);
    endpoints.MapControllers().RequireCors(WebApiOptionsExtensions.IngosDefaultCorsPolicy);
});

// app.UseStaticFiles();
app.UseIngosSwagger();

app.Run();
#pragma warning restore SA1516