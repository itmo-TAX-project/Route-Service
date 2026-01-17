using Application.DTO;

namespace Application.Repositories;

public interface IRouteRepository
{
    public Task SaveAsync(RouteDto route, CancellationToken cancellationToken);

    public Task<RouteDto?> GetAsync(long routeId, CancellationToken ct);
}