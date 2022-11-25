using Role.Infrastructure;
using AutoFixture;
using Role.Application.Dependencies;
using Role.Application.Features.Role.UpdateRolePermissions;

namespace Role.Application.Tests.Handlers.Role;

public class UpdateRolePermissionsHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task UpdateRolePermissions_Success()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<UpdateRolePermissions>();

            context.Permissions.Add(_fixture.CreatePermission(request.Role.PermissionIds.First()));
            context.Roles.Add(_fixture.CreateRole(request.Role.Id));
            context.SaveChanges();

            _fixture.Inject<IRoleDbContext>(context);
            var sut = _fixture.Create<UpdateRolePermissionsHandler>();

            var result = await sut.Handle(request, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }
    }
}
