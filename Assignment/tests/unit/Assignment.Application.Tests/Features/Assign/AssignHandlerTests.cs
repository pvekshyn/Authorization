using Assignment.Application.Features.Assign;
using AutoFixture;

namespace Assignment.Application.Tests.Features.Assign;

public class AssignHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task Assign_Success()
    {
        var request = _fixture.Create<Application.Features.Assign.Assign>();

        var sut = _fixture.Create<AssignHandler>();

        var result = await sut.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
    }
}
