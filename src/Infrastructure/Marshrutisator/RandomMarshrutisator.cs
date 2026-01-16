using Application.DTO;
using Application.DTO.Enums;
using Application.Ports;

using System.Security.Cryptography; // чтобы не жаловался CA, а то разнылся тут. Сопля

namespace Infrastructure.Marshrutisator;

public class RandomMarshrutisator : IMarshrutisator
{
    public Task<RawRouteDto> CalculateAsync(PointDto pickup, PointDto dropoff, CancellationToken ct)
    {
        int segmentsCount = RandomNumberGenerator.GetInt32(1, 10);
        int pointsCount = segmentsCount + 1;

        var polyline = new List<PointDto>(pointsCount);
        var types = new List<SegmentType>(segmentsCount);

        for (int i = 0; i < pointsCount; i++)
        {
            double t = (double)i / segmentsCount;

            double lat = pickup.Latitude + ((dropoff.Latitude - pickup.Latitude) * t);
            double lon = pickup.Longitude + ((dropoff.Longitude - pickup.Longitude) * t);

            polyline.Add(new PointDto()
                {
                    Latitude = lat,
                    Longitude = lon,
                });

            if (i >= segmentsCount) continue;
            SegmentType[] segmentTypes = Enum.GetValues<SegmentType>();
            SegmentType type = segmentTypes[RandomNumberGenerator.GetInt32(segmentTypes.Length)];
            types.Add(type);
        }

        return Task.FromResult(new RawRouteDto()
        {
            PathPolyline = polyline,
            SegmentTypes = types,
        });
    }
}