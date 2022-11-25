using Common.SDK;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly;

namespace Common.Application.Behaviors;

public class ExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TResponse : Result
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ExceptionBehavior<TRequest, TResponse>> _logger;

    public ExceptionBehavior(ILogger<ExceptionBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        TResponse response;
        try
        {
            var retryPolicy = Policy
              .Handle<Exception>()
              .RetryAsync(2);

            response = await retryPolicy.ExecuteAsync(async () =>
                await next()
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            response = (TResponse)Activator.CreateInstance(typeof(TResponse), 500);
        }

        return response;
    }
}

