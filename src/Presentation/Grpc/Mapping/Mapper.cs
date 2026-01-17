using Application.DTO;
using Application.DTO.Enums;

namespace Presentation.Grpc.Mapping;

public static class Mapper
{
    public static Routes.Client.Contracts.SegmentType ToRpc(this SegmentType type)
    {
        return type switch
        {
            SegmentType.Default => Routes.Client.Contracts.SegmentType.Default,
            SegmentType.Road => Routes.Client.Contracts.SegmentType.Road,
            SegmentType.Turn => Routes.Client.Contracts.SegmentType.Turn,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };
    }

    public static Routes.Client.Contracts.Point ToRpc(this PointDto dto)
    {
        return new Routes.Client.Contracts.Point
        {
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
        };
    }

    public static Routes.Client.Contracts.Segment ToRpc(this SegmentDto dto)
    {
        return new Routes.Client.Contracts.Segment
        {
            Type = dto.Type.ToRpc(),
            LengthM = dto.LengthM,
        };
    }

    public static Routes.Client.Contracts.Route ToRpc(this RouteDto dto)
    {
        var route = new Routes.Client.Contracts.Route
        {
            RouteId = dto.RouteId,
            DistanceM = dto.DistanceM,
            DurationS = dto.DurationS,
        };

        foreach (var p in dto.PathPolyline)
            route.PathPolyline.Add(p.ToRpc());

        foreach (var s in dto.Segments)
            route.Segments.Add(s.ToRpc());

        return route;
    }
}