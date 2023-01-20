namespace Common.Application.Dependencies
{
    public interface ICurrentUserCheckAccessService
    {
        Task<bool> CheckAccessAsync(Guid permissionId);
    }
}