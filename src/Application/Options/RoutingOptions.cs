using Application.DTO.Enums;

namespace Application.Options;

public class RoutingOptions
{
    public required Dictionary<SegmentType, double> RouteSpeed { get; set; }
}