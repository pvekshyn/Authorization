using AutoFixture;
using Role.Application.Features.Role.Create;

namespace Role.Application.Tests.Features.Role.Create;

public class CreateRoleHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task CreateRole_Success()
    {
        var request = _fixture.Create<CreateRole>();

        var permission = _fixture.CreatePermission(request.Role.PermissionIds.First());
        _dbContext.Permissions.Add(permission);
        _dbContext.SaveChanges();

        var sut = _fixture.Create<CreateRoleHandler>();

        var result = await sut.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);

        Assert.Single(_dbContext.Roles);
    }
}
