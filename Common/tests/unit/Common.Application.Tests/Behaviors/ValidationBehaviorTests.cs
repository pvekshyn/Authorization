using Common.Application.Behaviors;
using Common.SDK;
using AutoFixture;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;

namespace Common.Application.Tests.Behaviors;

public class ValidationBehaviorTests : ApplicationTestBase
{
    [Fact]
    public async Task Handle_ValidationError_Return422()
    {
        var request = _fixture.Create<TestRequest>();

        var failure = _fixture.Build<ValidationFailure>()
            .Without(x => x.CustomState)
            .Create();

        var validationResult = new ValidationResult(new[] {
            failure
        });

        var validatorMock = _fixture.Freeze<Mock<IValidator<TestRequest>>>();
        validatorMock
            .Setup(x => x.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(validationResult);

        var requestHandlerDelegate = _fixture.Create<Mock<RequestHandlerDelegate<Result>>>();
        requestHandlerDelegate.Setup(m => m())
            .Throws(new Exception());

        var sut = _fixture.Create<ValidationBehavior<TestRequest, Result>>();

        var result = await sut.Handle(request, CancellationToken.None, requestHandlerDelegate.Object);

        Assert.False(result.IsSuccess);
        Assert.Equal(422, result.Status);
        Assert.Single(result.Errors);
        Assert.Equal(failure.PropertyName, result.Errors.Single().Field);
        Assert.Equal(failure.ErrorCode, result.Errors.Single().Code);
    }
}
