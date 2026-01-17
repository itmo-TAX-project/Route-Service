using Application.Options;
using Application.Services;
using Application.Services.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ApplicationExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRouteQueryService, RouteQueryService>();
        services.AddScoped<IRouteService, RouteService>();

        services.AddOptions<RoutingOptions>()
            .Bind(configuration.GetSection("Routing"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}