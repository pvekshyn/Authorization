using AutoFixture;
using Role.Application.Features.Permission.Create;

namespace Role.Application.Tests.Features.Permission.Create;
public class CreatePermissionIdempotencyCheckerTests : ApplicationTestBase
{
    private readonly CreatePermissionIdempotencyCheck _sut;

    public CreatePermissionIdempotencyCheckerTests()
    {
        _sut = _fixture.Create<CreatePermissionIdempotencyCheck>();
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_True()
    {
        var request = _fixture.Create<CreatePermission>();

        _dbContext.Permissions.Add(_fixture.CreatePermission(request.Permission.Id, request.Permission.Name));
        _dbContext.SaveChanges();

        var result = await _sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_False()
    {
        var request = _fixture.Create<CreatePermission>();

        var result = await _sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.False(result);
    }
}
