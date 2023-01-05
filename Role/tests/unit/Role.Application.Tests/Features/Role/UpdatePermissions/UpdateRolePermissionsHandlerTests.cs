using AutoFixture;
using Role.Application.Features.Role.UpdatePermissions;

namespace Role.Application.Tests.Features.Role.UpdatePermissions;

public class UpdateRolePermissionsHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task UpdateRolePermissions_Success()
    {
        var request = _fixture.Create<UpdateRolePermissions>();

        var sut = _fixture.Create<UpdateRolePermissionsHandler>();

        var result = await sut.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
    }
}
