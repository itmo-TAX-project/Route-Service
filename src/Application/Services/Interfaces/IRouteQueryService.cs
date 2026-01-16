using Application.DTO;

namespace Application.Services.Interfaces;

public interface IRouteQueryService
{
    public Task<RouteDto?> GetRoute(long routeId, CancellationToken ct);
}