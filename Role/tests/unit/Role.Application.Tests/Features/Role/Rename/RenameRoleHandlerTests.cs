using AutoFixture;
using Role.Application.Features.Role.Rename;

namespace Role.Application.Tests.Features.Role.Rename;

public class RenameRoleHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task RenameRole_Success()
    {
        var request = _fixture.Create<RenameRole>();

        var sut = _fixture.Create<RenameRoleHandler>();

        var result = await sut.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
    }
}
