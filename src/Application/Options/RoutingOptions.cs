using Application.DTO.Enums;

namespace Application.Options;

public class RoutingOptions
{
    public Dictionary<SegmentType, double> RouteSpeed { get; init; } = new();
}