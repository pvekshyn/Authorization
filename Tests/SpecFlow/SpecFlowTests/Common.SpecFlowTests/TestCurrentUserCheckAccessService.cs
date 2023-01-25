using Common.Application.Dependencies;

namespace Role.Integration.Tests
{
    internal class TestCurrentUserCheckAccessService : ICurrentUserCheckAccessService
    {
        public async Task<bool> CheckAccessAsync(Guid permissionId)
        {
            return true;
        }
    }
}
