using AutoFixture;
using Role.Application.Features.Role.Delete;

namespace Role.Application.Tests.Features.Role.Delete;

public class DeletePermissionHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task DeleteRole_Success()
    {
        var request = _fixture.Create<DeleteRole>();

        _dbContext.Roles.Add(_fixture.CreateRole(request.Id));
        _dbContext.SaveChanges();

        var sut = _fixture.Create<DeleteRoleHandler>();

        var result = await sut.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(_dbContext.Roles);
    }
}
