using AutoFixture;
using Role.Application.Features.Role.Create;

namespace Role.Application.Tests.Features.Role.Create;

public class CreateRoleHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task CreateRole_Success()
    {
        var request = _fixture.Create<CreateRole>();

        var sut = _fixture.Create<CreateRoleHandler>();

        var result = await sut.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
    }
}
