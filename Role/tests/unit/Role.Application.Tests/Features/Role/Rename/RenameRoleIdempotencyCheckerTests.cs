using AutoFixture;
using Role.Application.Features.Role.Rename;

namespace Role.Application.Tests.Features.Role.Rename;
public class RenameRoleIdempotencyCheckerTests : ApplicationTestBase
{
    [Fact]
    public async Task IsOperationAlreadyApplied_True()
    {
        var request = _fixture.Create<RenameRole>();

        _dbContext.Roles.Add(_fixture.CreateRole(request.Role.Id, roleName: request.Role.Name));
        _dbContext.SaveChanges();

        var sut = _fixture.Create<RenameRoleIdempotencyCheck>();

        var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_False()
    {
        var request = _fixture.Create<RenameRole>();

        var sut = _fixture.Create<RenameRoleIdempotencyCheck>();

        var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.False(result);
    }
}
