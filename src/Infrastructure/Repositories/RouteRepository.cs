using Application.DTO;
using Application.Ports;
using Npgsql;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Infrastructure.Repositories;

public class RouteRepository : IRouteRepository
{
    private readonly NpgsqlDataSource _datasource;

    public RouteRepository(NpgsqlDataSource datasource)
    {
        _datasource = datasource;
    }

    public async Task SaveAsync(RouteDto route, CancellationToken cancellationToken)
    {
        const string sql = """
                           INSERT INTO routes (
                               route_id,
                               path_polyline,
                               distance_m,
                               duration_s,
                               segments
                           )
                           VALUES (
                               @route_id,
                               @path_polyline,
                               @distance_m,
                               @duration_s,
                               @segments
                           )
                           ON CONFLICT (route_id) DO UPDATE SET
                               path_polyline = EXCLUDED.path_polyline,
                               distance_m    = EXCLUDED.distance_m,
                               duration_s    = EXCLUDED.duration_s,
                               segments      = EXCLUDED.segments;
                           """;

        var pathPolylineJson = JsonSerializer.Serialize(route.PathPolyline);
        var segmentsJson = JsonSerializer.Serialize(route.Segments);

        await using var connection = await _datasource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection).ConfigureAwait(false);

        command.Parameters.AddWithValue("route_id", route.RouteId);
        command.Parameters.AddWithValue("path_polyline", pathPolylineJson);
        command.Parameters.AddWithValue("distance_m", route.DistanceM);
        command.Parameters.AddWithValue("duration_s", route.DurationS);
        command.Parameters.AddWithValue("segments", segmentsJson);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<RouteDto?> GetAsync(long routeId, CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT route_id, path_polyline, distance_m, duration_s, segments
                           FROM routes
                           WHERE route_id = @route_id;
                           """;

        await using NpgsqlConnection connection = await _datasource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("route_id", routeId);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        var id = reader.GetInt64(0);
        var polylineJson = reader.GetString(1);
        var distanceM = reader.GetDouble(2);
        var durationS = reader.GetInt64(3);
        var segmentsJson = reader.GetString(4);

        var polyline = JsonSerializer.Deserialize<List<PointDto>>(polylineJson) ?? [];
        var segments = JsonSerializer.Deserialize<List<SegmentDto>>(segmentsJson) ?? [];

        return new RouteDto()
        {
            RouteId = id,
            PathPolyline = polyline,
            DistanceM = distanceM,
            DurationS = durationS,
            Segments = segments,
        };
    }
}
