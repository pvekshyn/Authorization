using Authorization.Infrastructure.DataAccess.Read;
using Common.SDK;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Role.SDK.DTO;

namespace Authorization.Application.Features.Role
{
    public class GetRole : IRequest<Result<RoleDto>>
    {
        public Guid Id { get; init; }
        public GetRole(Guid id)
        {
            Id = id;
        }
    }
    public class GetRoleHandler : IRequestHandler<GetRole, Result<RoleDto>>
    {
        IAuthorizationDbContext _dbContext;

        public GetRoleHandler(IAuthorizationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<RoleDto>> Handle(GetRole request, CancellationToken cancellationToken)
        {
            var Role = await _dbContext.Roles
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (Role is null)
                return Result<RoleDto>.NotFound();

            return Result<RoleDto>.Ok(MapToDto(Role));
        }

        private static RoleDto MapToDto(Domain.Role Role)
        {
            return new RoleDto
            {
                Id = Role.Id,
                Name = Role.Name
            };
        }
    }
}
