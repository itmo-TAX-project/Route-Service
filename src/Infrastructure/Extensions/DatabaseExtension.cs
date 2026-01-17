using Infrastructure.Database.Migrations;
using Infrastructure.Database.Options;
using Itmo.Dev.Platform.Persistence.Abstractions.Extensions;
using Itmo.Dev.Platform.Persistence.Postgres.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Extensions;

public static class DatabaseExtension
{
    public static IServiceCollection AddPostgresPersistence(this IServiceCollection services)
    {
        services.AddPlatformPersistence(
            persistence => persistence.UsePostgres(
                postgres => postgres.WithConnectionOptions(
                        builder =>
                        {
                            DatabaseOptions options = services.BuildServiceProvider()
                                .GetRequiredService<IOptions<DatabaseOptions>>().Value;

                            builder.Configure(connectionOptions =>
                            {
                                connectionOptions.Host = options.Host;
                                connectionOptions.Port = options.Port;
                                connectionOptions.Database = options.Database;
                                connectionOptions.Username = options.Username;
                                connectionOptions.Password = options.Password;
                                connectionOptions.SslMode = options.SslMode;
                            });
                        })
                    .WithMigrationsFrom(typeof(CreateRoutesMigration).Assembly)));

        return services;
    }
}
