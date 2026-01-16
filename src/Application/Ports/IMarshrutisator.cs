using Application.DTO;

namespace Application.Ports;

public interface IMarshrutisator
{
    public Task<RawRouteDto> CalculateAsync(PointDto pickup, PointDto dropoff, CancellationToken ct);
}