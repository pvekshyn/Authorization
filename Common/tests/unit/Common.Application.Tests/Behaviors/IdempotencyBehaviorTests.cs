using Common.Application.Behaviors;
using Common.Application.Idempotency;
using Common.SDK;
using AutoFixture;
using MediatR;
using Moq;

namespace Common.Application.Tests.Behaviors;

public class IdempotencyBehaviorTests : ApplicationTestBase
{
    [Fact]
    public async Task Handle_OperationAlreadyApplied_Return200()
    {
        var request = _fixture.Create<TestRequest>();

        var idempotencyCheckerMock = _fixture.Freeze<Mock<IIdempotencyCheck<TestRequest>>>();
        idempotencyCheckerMock
            .Setup(x => x.IsOperationAlreadyAppliedAsync(request, CancellationToken.None))
            .ReturnsAsync(true);

        var requestHandlerDelegate = _fixture.Create<Mock<RequestHandlerDelegate<Result>>>();
        requestHandlerDelegate.Setup(m => m())
            .Throws(new Exception());

        var sut = _fixture.Create<IdempotencyBehavior<TestRequest, Result>>();

        var result = await sut.Handle(request, CancellationToken.None, requestHandlerDelegate.Object);

        Assert.True(result.IsSuccess);
        Assert.Equal(204, result.Status);
    }
}
