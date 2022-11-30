using AutoFixture;
using Role.Application.Features.Permission.DeletePermission;

namespace Role.Application.Tests.Features.Permission.Delete;
public class DeletePermissionIdempotencyCheckerTests : ApplicationTestBase
{
    private readonly DeletePermissionIdempotencyCheck _sut;

    public DeletePermissionIdempotencyCheckerTests()
    {
        _sut = _fixture.Create<DeletePermissionIdempotencyCheck>();
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_True()
    {
        var request = _fixture.Create<DeletePermission>();

        var result = await _sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_False()
    {
        var request = _fixture.Create<DeletePermission>();

        _dbContext.Permissions.Add(_fixture.CreatePermission(request.PermissionId));
        _dbContext.SaveChanges();

        var result = await _sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.False(result);
    }
}
