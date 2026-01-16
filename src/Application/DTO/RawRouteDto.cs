using Application.DTO.Enums;

namespace Application.DTO;

public class RawRouteDto
{
    public required IReadOnlyList<PointDto> PathPolyline { get; set; }

    public required IReadOnlyList<SegmentType> SegmentTypes { get; set; }
}