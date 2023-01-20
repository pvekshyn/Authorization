using Common.Application.Authorization;
using Common.SDK;
using MediatR;

namespace Common.Application.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TResponse : Result
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IAuthorizationCheck<TRequest>> _authorizationChecks;

    public AuthorizationBehavior(IEnumerable<IAuthorizationCheck<TRequest>> authorizationChecks)
    {
        _authorizationChecks = authorizationChecks;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var authorizationCheck = _authorizationChecks.FirstOrDefault();

        if (authorizationCheck != null)
        {
            var result = await authorizationCheck.CheckAccessAsync();
            if (!result)
                return (TResponse)Activator.CreateInstance(typeof(TResponse), 403);
        }
        return await next();
    }
}

