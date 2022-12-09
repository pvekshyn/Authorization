using Authorization.Infrastructure.DataAccess.Read;
using Common.SDK;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Role.SDK.DTO;

namespace Authorization.Application.Features.Permission
{
    public class GetPermission : IRequest<Result<PermissionDto>>
    {
        public Guid Id { get; init; }
        public GetPermission(Guid id)
        {
            Id = id;
        }
    }
    public class GetPermissionHandler : IRequestHandler<GetPermission, Result<PermissionDto>>
    {
        IAuthorizationDbContext _dbContext;

        public GetPermissionHandler(IAuthorizationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<PermissionDto>> Handle(GetPermission request, CancellationToken cancellationToken)
        {
            var permission = await _dbContext.Permissions
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (permission is null)
                return Result<PermissionDto>.NotFound();

            return Result<PermissionDto>.Ok(MapToDto(permission));
        }

        private static PermissionDto MapToDto(Domain.Permission permission)
        {
            return new PermissionDto
            {
                Id = permission.Id,
                Name = permission.Name
            };
        }
    }
}
