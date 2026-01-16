using Application.DTO;
using Application.DTO.Enums;
using Application.Options;
using Application.Ports;
using Application.Services.Interfaces;

namespace Application.Services;

public class RouteService : IRouteService
{
    private static long _routeIdSeed = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    private readonly IMarshrutisator _marshrutisator;

    private readonly IRouteRepository _routeRepository;

    private readonly RoutingOptions _options;

    public RouteService(IMarshrutisator marshrutisator, IRouteRepository routeRepository, RoutingOptions options)
    {
        _marshrutisator = marshrutisator;
        _routeRepository = routeRepository;
        _options = options;
    }

    public async Task<long> CalculateRoute(PointDto pickup, PointDto dropoff, CancellationToken ct)
    {
        RawRouteDto raw = await _marshrutisator.CalculateAsync(pickup, dropoff, ct).ConfigureAwait(false);

        if (raw.PathPolyline is null || raw.PathPolyline.Count < 2)
            throw new InvalidOperationException("PathPolyline must contain at least 2 points.");

        int expectedSegments = raw.PathPolyline.Count - 1;

        if (raw.SegmentTypes is null || raw.SegmentTypes.Count != expectedSegments)
            throw new InvalidOperationException("SegmentTypes count must be PathPolyline.Count - 1.");

        var segments = new List<SegmentDto>(expectedSegments);

        double totalDistance = 0.0;
        double totalSeconds = 0.0;

        for (int i = 0; i < expectedSegments; i++)
        {
            PointDto a = raw.PathPolyline[i];
            PointDto b = raw.PathPolyline[i + 1];

            double lengthM = EuclideanDistanceMeters(a, b);
            SegmentType type = raw.SegmentTypes[i];

            segments.Add(new SegmentDto()
                {
                    Type = type,
                    LengthM = lengthM,
                });

            totalDistance += lengthM;

            if (!_options.RouteSpeed.TryGetValue(type, out double speedMps) || speedMps <= 0)
                throw new InvalidOperationException($"Speed for segment type '{type}' is not configured or invalid.");

            totalSeconds += lengthM / speedMps;
        }

        long routeId = Interlocked.Increment(ref _routeIdSeed);

        var route = new RouteDto()
        {
            RouteId = routeId,
            PathPolyline = raw.PathPolyline,
            DistanceM = totalDistance,
            DurationS = (long)Math.Ceiling(totalSeconds),
            Segments = segments,
        };

        await _routeRepository.SaveAsync(route, ct).ConfigureAwait(false);

        return routeId;
    }

    private static double EuclideanDistanceMeters(PointDto a, PointDto b)
    {
        const double metersPerDegreeLat = 111_320.0;

        double lat1 = a.Latitude;
        double lon1 = a.Longitude;
        double lat2 = b.Latitude;
        double lon2 = b.Longitude;

        double meanLatRad = (lat1 + lat2) * 0.5 * (Math.PI / 180.0);

        double dx = (lon2 - lon1) * metersPerDegreeLat * Math.Cos(meanLatRad);
        double dy = (lat2 - lat1) * metersPerDegreeLat;

        return Math.Sqrt((dx * dx) + (dy * dy));
    }
}