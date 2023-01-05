using AutoFixture;
using Moq;
using Role.Application.Dependencies;
using Role.Application.Features.Role.Rename;

namespace Role.Application.Tests.Features.Role.Rename;
public class RenameRoleIdempotencyCheckerTests : ApplicationTestBase
{
    [Fact]
    public async Task IsOperationAlreadyApplied_True()
    {
        var request = _fixture.Create<RenameRole>();

        var roleRepository = _fixture.Freeze<Mock<IRoleRepository>>();
        roleRepository
            .Setup(x => x.AnyAsync(request.Role.Id, request.Role.Name, CancellationToken.None))
            .ReturnsAsync(true);

        var sut = _fixture.Create<RenameRoleIdempotencyCheck>();

        var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_False()
    {
        var request = _fixture.Create<RenameRole>();

        var roleRepository = _fixture.Freeze<Mock<IRoleRepository>>();
        roleRepository
            .Setup(x => x.AnyAsync(request.Role.Id, request.Role.Name, CancellationToken.None))
            .ReturnsAsync(false);

        var sut = _fixture.Create<RenameRoleIdempotencyCheck>();

        var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.False(result);
    }
}
