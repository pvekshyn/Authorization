using AutoFixture;
using Moq;
using Role.Application.Dependencies;
using Role.Application.Features.Role.Create;

namespace Role.Application.Tests.Features.Role.Create;
public class CreateRoleIdempotencyCheckerTests : ApplicationTestBase
{
    [Fact]
    public async Task IsOperationAlreadyApplied_True()
    {
        var request = _fixture.Create<CreateRole>();

        var roleRepository = _fixture.Freeze<Mock<IRoleRepository>>();
        roleRepository
            .Setup(x => x.AnyAsync(request.Role.Id, CancellationToken.None))
            .ReturnsAsync(true);

        var sut = _fixture.Create<CreateRoleIdempotencyCheck>();

        var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_False()
    {
        var request = _fixture.Create<CreateRole>();

        var roleRepository = _fixture.Freeze<Mock<IRoleRepository>>();
        roleRepository
            .Setup(x => x.AnyAsync(request.Role.Id, CancellationToken.None))
            .ReturnsAsync(false);

        var sut = _fixture.Create<CreateRoleIdempotencyCheck>();

        var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.False(result);
    }
}
