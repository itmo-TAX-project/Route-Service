using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Extensions;

public static class GrpcExtension
{
    public static IServiceCollection AddGrpcServer(this IServiceCollection services)
    {
        services.AddGrpc();
        services.AddGrpcReflection();

        return services;
    }
}