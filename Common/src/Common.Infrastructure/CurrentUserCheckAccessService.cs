using Common.Application.Dependencies;

namespace Common.Infrastructure
{
    internal class CurrentUserCheckAccessService : ICurrentUserCheckAccessService
    {
        private readonly GrpcCheckAccessService.GrpcCheckAccessServiceClient _grpcCheckAccessClient;
        private readonly ICurrentContext _currentContext;

        public CurrentUserCheckAccessService(
            GrpcCheckAccessService.GrpcCheckAccessServiceClient grpcCheckAccessClient,
            ICurrentContext currentContext)
        {
            _grpcCheckAccessClient = grpcCheckAccessClient;
            _currentContext = currentContext;
        }
        public async Task<bool> CheckAccessAsync(Guid permissionId)
        {
            var request = new GrpcCheckAccessRequest
            {
                UserId = _currentContext.UserId.ToString(),
                PermissionId = permissionId.ToString()
            };

            var result = await _grpcCheckAccessClient.CheckAccessAsync(request);
            return result.HasAccess;
        }
    }
}
