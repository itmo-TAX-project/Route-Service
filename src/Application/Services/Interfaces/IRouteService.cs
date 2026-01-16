using Application.DTO;

namespace Application.Services.Interfaces;

public interface IRouteService
{
    public Task<long> CalculateRoute(PointDto pickup, PointDto dropoff, CancellationToken ct);
}