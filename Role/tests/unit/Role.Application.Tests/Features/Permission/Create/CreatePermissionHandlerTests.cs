using AutoFixture;
using Role.Application.Features.Permission.Create;

namespace Role.Application.Tests.Features.Permission.Create;
public class CreatePermissionHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task CreatePermission_Success()
    {
        var request = _fixture.Create<CreatePermission>();

        var sut = _fixture.Create<CreatePermissionHandler>();

        var result = await sut.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Single(_dbContext.Permissions);
    }
}
