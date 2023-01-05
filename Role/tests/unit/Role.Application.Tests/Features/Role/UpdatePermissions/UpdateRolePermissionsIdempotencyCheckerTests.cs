using AutoFixture;
using Moq;
using Role.Application.Dependencies;
using Role.Application.Features.Role.UpdatePermissions;

namespace Role.Application.Tests.Features.Role.UpdatePermissions;
public class UpdateRolePermissionsIdempotencyCheckerTests : ApplicationTestBase
{
    [Fact]
    public async Task IsOperationAlreadyApplied_True()
    {
        var request = _fixture.Create<UpdateRolePermissions>();
        var role = _fixture.CreateRole(request.Role.Id, request.Role.PermissionIds);

        var roleRepository = _fixture.Freeze<Mock<IRoleRepository>>();
        roleRepository
            .Setup(x => x.GetAsync(request.Role.Id, CancellationToken.None))
            .ReturnsAsync(role);

        var sut = _fixture.Create<UpdateRolePermissionsIdempotencyCheck>();

        var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_False()
    {
        var request = _fixture.Create<UpdateRolePermissions>();

        var sut = _fixture.Create<UpdateRolePermissionsIdempotencyCheck>();

        var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.False(result);
    }
}
