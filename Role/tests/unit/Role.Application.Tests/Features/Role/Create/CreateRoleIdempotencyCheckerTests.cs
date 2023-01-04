using AutoFixture;
using Role.Application.Features.Role.Create;

namespace Role.Application.Tests.Features.Role.Create;
public class CreateRoleIdempotencyCheckerTests : ApplicationTestBase
{
    private readonly CreateRoleIdempotencyCheck _sut;

    public CreateRoleIdempotencyCheckerTests()
    {
        _sut = _fixture.Create<CreateRoleIdempotencyCheck>();
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_True()
    {
        var request = _fixture.Create<CreateRole>();

        _dbContext.Roles.Add(_fixture.CreateRole(request.Role.Id, roleName: request.Role.Name));
        _dbContext.SaveChanges();

        var result = await _sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_False()
    {
        var request = _fixture.Create<CreateRole>();

        var result = await _sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.False(result);
    }
}
