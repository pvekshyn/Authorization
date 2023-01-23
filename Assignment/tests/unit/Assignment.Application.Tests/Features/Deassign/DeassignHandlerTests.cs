using Assignment.Application.Features.Deassign;
using AutoFixture;

namespace Assignment.Application.Tests.Features.Deassign;

public class DeassignHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task Deassign_Success()
    {
        var request = _fixture.Create<Application.Features.Deassign.Deassign>();

        var sut = _fixture.Create<DeassignHandler>();

        var result = await sut.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
    }
}
