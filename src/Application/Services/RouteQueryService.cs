using Application.DTO;
using Application.Repositories;
using Application.Services.Interfaces;

namespace Application.Services;

public class RouteQueryService : IRouteQueryService
{
    private readonly IRouteRepository _routeRepository;

    public RouteQueryService(IRouteRepository routeRepository)
    {
        _routeRepository = routeRepository;
    }

    public Task<RouteDto?> GetRoute(long routeId, CancellationToken ct)
    {
        return _routeRepository.GetAsync(routeId, ct);
    }
}