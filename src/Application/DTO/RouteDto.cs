namespace Application.DTO;

public class RouteDto
{
    public long RouteId { get; set; }

    public required IReadOnlyList<PointDto> PathPolyline { get; set; }

    public double DistanceM { get; set; }

    public long DurationS { get; set; }

    public required IReadOnlyList<SegmentDto> Segments { get; set; }
}