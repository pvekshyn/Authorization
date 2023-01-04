using AutoFixture;
using Role.Application.Features.Role.UpdatePermissions;

namespace Role.Application.Tests.Features.Role.UpdatePermissions;
public class UpdateRolePermissionsIdempotencyCheckerTests : ApplicationTestBase
{
    private readonly UpdateRolePermissionsIdempotencyCheck _sut;

    public UpdateRolePermissionsIdempotencyCheckerTests()
    {
        _sut = _fixture.Create<UpdateRolePermissionsIdempotencyCheck>();
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_True()
    {
        var request = _fixture.Create<UpdateRolePermissions>();

        var role = _fixture.CreateRole(request.Role.Id, permissionIds: request.Role.PermissionIds);

        _dbContext.Permissions.AddRange(role.Permissions);
        _dbContext.Roles.Add(role);
        _dbContext.SaveChanges();

        var result = await _sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_False()
    {
        var request = _fixture.Create<UpdateRolePermissions>();

        var result = await _sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.False(result);
    }
}
