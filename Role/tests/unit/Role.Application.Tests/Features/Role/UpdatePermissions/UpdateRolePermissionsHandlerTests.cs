using AutoFixture;
using Role.Application.Features.Role.UpdateRolePermissions;

namespace Role.Application.Tests.Features.Role.UpdatePermissions;

public class UpdateRolePermissionsHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task UpdateRolePermissions_Success()
    {
        var request = _fixture.Create<UpdateRolePermissions>();

        _dbContext.Permissions.Add(_fixture.CreatePermission(request.Role.PermissionIds.First()));
        _dbContext.Roles.Add(_fixture.CreateRole(request.Role.Id));
        _dbContext.SaveChanges();

        var sut = _fixture.Create<UpdateRolePermissionsHandler>();

        var result = await sut.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
    }
}
