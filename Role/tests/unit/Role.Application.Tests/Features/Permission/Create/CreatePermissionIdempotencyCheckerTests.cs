using AutoFixture;
using Moq;
using Role.Application.Dependencies;
using Role.Application.Features.Permission.Create;

namespace Role.Application.Tests.Features.Permission.Create;
public class CreatePermissionIdempotencyCheckerTests : ApplicationTestBase
{
    public CreatePermissionIdempotencyCheckerTests()
    {
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_True()
    {
        var request = _fixture.Create<CreatePermission>();

        var permissionRepository = _fixture.Freeze<Mock<IPermissionRepository>>();
        permissionRepository
            .Setup(x => x.AnyAsync(request.Permission.Id, CancellationToken.None))
            .ReturnsAsync(true);

        var _sut = _fixture.Create<CreatePermissionIdempotencyCheck>();

        var result = await _sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_False()
    {
        var request = _fixture.Create<CreatePermission>();

        var permissionRepository = _fixture.Freeze<Mock<IPermissionRepository>>();
        permissionRepository
            .Setup(x => x.AnyAsync(request.Permission.Id, CancellationToken.None))
            .ReturnsAsync(false);

        var _sut = _fixture.Create<CreatePermissionIdempotencyCheck>();

        var result = await _sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.False(result);
    }
}
