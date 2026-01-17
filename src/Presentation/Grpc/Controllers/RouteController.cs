using Application.DTO;
using Application.Services.Interfaces;
using Grpc.Core;
using Presentation.Grpc.Mapping;
using Routes.Client.Contracts;

namespace Presentation.Grpc.Controllers;

public sealed class RouteController(IRouteService routeService, IRouteQueryService routeQueryService)
    : RouteService.RouteServiceBase
{
    public override async Task<CalculateRouteResponse> CalculateRoute(
        CalculateRouteRequest request,
        ServerCallContext context)
    {
        long routeId = await routeService.CalculateRoute(
            new PointDto()
            {
                Latitude = request.Pickup.Latitude,
                Longitude = request.Pickup.Longitude,
            },
            new PointDto()
            {
                Latitude = request.Dropoff.Latitude,
                Longitude = request.Dropoff.Longitude,
            },
            context.CancellationToken).ConfigureAwait(false);

        return new CalculateRouteResponse { RouteId = routeId };
    }

    public override async Task<GetRouteResponse> GetRoute(
        GetRouteRequest request,
        ServerCallContext context)
    {
        RouteDto? route = await routeQueryService.GetRoute(request.RouteId, context.CancellationToken).ConfigureAwait(false);

        return route is null ?
            throw new RpcException(new Status(StatusCode.NotFound, "Route not found")) :
            new GetRouteResponse { Route = route.ToRpc() };
    }
}