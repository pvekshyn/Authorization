using Role.Infrastructure;
using AutoFixture;
using Role.Application.Dependencies;
using Role.Application.Features.Role.UpdateRolePermissions;

namespace Role.Application.Tests.Features.Role.UpdatePermissions;
public class UpdateRolePermissionsIdempotencyCheckerTests : ApplicationTestBase
{
    [Fact]
    public async Task IsOperationAlreadyApplied_True()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<UpdateRolePermissions>();

            var role = _fixture.CreateRole(request.Role.Id);
            var permissions = request.Role.PermissionIds.Select(x => _fixture.CreatePermission(x)).ToList();
            role.ReplacePermissions(permissions);
            foreach (var permission in permissions)
            {
                context.Permissions.Add(permission);
            }

            context.Roles.Add(role);
            context.SaveChanges();

            _fixture.Inject<IRoleDbContext>(context);
            var sut = _fixture.Create<UpdateRolePermissionsIdempotencyCheck>();

            var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

            Assert.True(result);
        }
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_False()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<UpdateRolePermissions>();

            _fixture.Inject<IRoleDbContext>(context);
            var sut = _fixture.Create<UpdateRolePermissionsIdempotencyCheck>();

            var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

            Assert.False(result);
        }
    }
}
