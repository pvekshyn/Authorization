namespace Common.Application.Authorization;
public interface IAuthorizationCheck<TRequest>
{
    public Task<bool> CheckAccessAsync();
}
