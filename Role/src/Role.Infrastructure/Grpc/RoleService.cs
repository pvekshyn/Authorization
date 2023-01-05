using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Role.Infrastructure.Grpc
{
    public class RoleService : GrpcRoleService.GrpcRoleServiceBase
    {
        private readonly RoleDbContext _dbContext;

        public RoleService(RoleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task GetRoles(GrpcRolesRequest request, IServerStreamWriter<GrpcRole> responseStream, ServerCallContext context)
        {
            var roles = _dbContext.Roles
                .AsNoTracking()
                .Include(x => x.Permissions)
                .AsAsyncEnumerable();

            await foreach (var role in roles)
            {
                await responseStream.WriteAsync(MapFrom(role));
            }
        }

        public override async Task GetPermissions(GrpcPermissionsRequest request, IServerStreamWriter<GrpcPermission> responseStream, ServerCallContext context)
        {
            var permissions = _dbContext.Permissions
                .AsNoTracking()
                .AsAsyncEnumerable();

            await foreach (var permission in permissions)
            {
                await responseStream.WriteAsync(MapFrom(permission));
            }
        }

        private GrpcRole MapFrom(Domain.Role role)
        {
            var grpcRole = new GrpcRole()
            {
                Id = role.Id.ToString(),
                Name = role.Name
            };
            grpcRole.Permissions.AddRange(role.Permissions.Select(MapFrom));

            return grpcRole;
        }

        private GrpcPermission MapFrom(Domain.Permission permission)
        {
            return new GrpcPermission()
            {
                Id = permission.Id.ToString(),
                Name = permission.Name
            };
        }
    }
}