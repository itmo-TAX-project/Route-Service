using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Presentation.Grpc.Interceptors;

public sealed class ErrorHandlerInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context).ConfigureAwait(false);
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(MapStatusCode(ex), ex.Message));
        }
    }

    private static StatusCode MapStatusCode(Exception ex)
    {
        return ex switch
        {
            ArgumentException => StatusCode.InvalidArgument,
            KeyNotFoundException => StatusCode.NotFound,
            InvalidOperationException => StatusCode.FailedPrecondition,
            OperationCanceledException => StatusCode.Cancelled,
            NotSupportedException => StatusCode.Unimplemented,
            _ => StatusCode.Internal,
        };
    }
}