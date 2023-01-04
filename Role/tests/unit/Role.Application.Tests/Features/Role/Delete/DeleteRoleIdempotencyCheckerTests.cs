using AutoFixture;
using Role.Application.Features.Role.Delete;

namespace Role.Application.Tests.Features.Role.Delete;
public class DeleteRoleIdempotencyCheckerTests : ApplicationTestBase
{
    private readonly DeleteRoleIdempotencyCheck _sut;

    public DeleteRoleIdempotencyCheckerTests()
    {
        _sut = _fixture.Create<DeleteRoleIdempotencyCheck>();
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_True()
    {
        var request = _fixture.Create<DeleteRole>();

        var result = await _sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_False()
    {
        var request = _fixture.Create<DeleteRole>();

        _dbContext.Roles.Add(_fixture.CreateRole(request.Id));
        _dbContext.SaveChanges();

        var result = await _sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.False(result);
    }
}
