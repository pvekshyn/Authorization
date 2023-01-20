using Authorization.Infrastructure.DataAccess.Read;
using Grpc.Core;

namespace Authorization.Infrastructure.Grpc
{
    public class CheckAccessService : GrpcCheckAccessService.GrpcCheckAccessServiceBase
    {
        private IAccessRepository _repository;

        public CheckAccessService(IAccessRepository repository)
        {
            _repository = repository;
        }

        public override async Task<GrpcCheckAccessResult> CheckAccess(GrpcCheckAccessRequest request, ServerCallContext context)
        {
            var hasAccess = _repository.CheckAccess(Guid.Parse(request.UserId), Guid.Parse(request.PermissionId));
            return await Task.FromResult(new GrpcCheckAccessResult { HasAccess = hasAccess });
        }
    }
}