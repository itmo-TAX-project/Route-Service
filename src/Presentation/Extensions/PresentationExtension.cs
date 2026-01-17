using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Grpc.Interceptors;

namespace Presentation.Extensions;

public static class PresentationExtension
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddGrpcServer();
        services.AddSingleton<ErrorHandlerInterceptor>();

        return services;
    }
}