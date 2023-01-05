using AutoFixture;
using Role.Application.Features.Role.Delete;

namespace Role.Application.Tests.Features.Role.Delete;

public class DeletePermissionHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task DeleteRole_Success()
    {
        var request = _fixture.Create<DeleteRole>();

        var sut = _fixture.Create<DeleteRoleHandler>();

        var result = await sut.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
    }
}
