using AutoFixture;
using Role.Application.Features.Permission.Delete;

namespace Role.Application.Tests.Features.Permission.Delete;

public class DeletePermissionHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task DeletePermission_Success()
    {
        var request = _fixture.Create<DeletePermission>();

        var sut = _fixture.Create<DeletePermissionHandler>();

        var result = await sut.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
    }
}
