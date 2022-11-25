using Common.Application.Behaviors;
using Common.SDK;
using AutoFixture;
using MediatR;
using Moq;

namespace Common.Application.Tests.Behaviors;

public class ExceptionBehaviorTests : ApplicationTestBase
{
    [Fact]
    public async Task Handle_Exception_Return500()
    {
        var exceptionMessage = _fixture.Create<string>();
        var request = _fixture.Create<TestRequest>();

        var requestHandlerDelegate = _fixture.Create<Mock<RequestHandlerDelegate<Result>>>();
        requestHandlerDelegate.Setup(m => m())
            .Throws(new Exception(exceptionMessage));

        var sut = _fixture.Create<ExceptionBehavior<TestRequest, Result>>();

        var result = await sut.Handle(request, CancellationToken.None, requestHandlerDelegate.Object);

        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.Status);

        requestHandlerDelegate.Verify(x => x(), Times.Exactly(3));
    }
}
