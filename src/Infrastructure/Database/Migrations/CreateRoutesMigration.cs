using FluentMigrator;

namespace Infrastructure.Database.Migrations;

[Migration(1, description: "Create routes table")]
public class CreateRoutesMigration : Migration
{
    public override void Up()
    {
        Execute.Sql(
            """
                CREATE TABLE routes
                (
                    route_id        BIGINT PRIMARY KEY,
                    path_polyline   JSONB NOT NULL,
                    distance_m      DOUBLE PRECISION NOT NULL,
                    duration_s      BIGINT NOT NULL,
                    segments        JSONB NOT NULL,
                    created_at      TIMESTAMPTZ NOT NULL DEFAULT now()
                );

                CREATE INDEX idx_routes_created_at ON routes(created_at);
            """);
    }

    public override void Down()
    {
        Execute.Sql(
            """
                DROP TABLE IF EXISTS routes;
            """);
    }
}