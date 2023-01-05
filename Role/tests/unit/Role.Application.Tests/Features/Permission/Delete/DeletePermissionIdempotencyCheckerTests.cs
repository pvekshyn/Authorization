using AutoFixture;
using Moq;
using Role.Application.Dependencies;
using Role.Application.Features.Permission.Delete;

namespace Role.Application.Tests.Features.Permission.Delete;
public class DeletePermissionIdempotencyCheckerTests : ApplicationTestBase
{
    public DeletePermissionIdempotencyCheckerTests()
    {
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_True()
    {
        var request = _fixture.Create<DeletePermission>();

        var permissionRepository = _fixture.Freeze<Mock<IPermissionRepository>>();
        permissionRepository
            .Setup(x => x.AnyAsync(request.PermissionId, CancellationToken.None))
            .ReturnsAsync(false);

        var sut = _fixture.Create<DeletePermissionIdempotencyCheck>();

        var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_False()
    {
        var request = _fixture.Create<DeletePermission>();

        var permissionRepository = _fixture.Freeze<Mock<IPermissionRepository>>();
        permissionRepository
            .Setup(x => x.AnyAsync(request.PermissionId, CancellationToken.None))
            .ReturnsAsync(true);

        var sut = _fixture.Create<DeletePermissionIdempotencyCheck>();

        var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.False(result);
    }
}
