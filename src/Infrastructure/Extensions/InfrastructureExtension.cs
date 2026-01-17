using Application.Ports;
using Infrastructure.Marshrutisator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddPostgresPersistence()
            .AddRepositories()
            .AddScoped<IMarshrutisator, RandomMarshrutisator>();
    }
}