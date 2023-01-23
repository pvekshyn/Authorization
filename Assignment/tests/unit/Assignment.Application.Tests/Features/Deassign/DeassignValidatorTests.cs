using Assignment.Application.Dependencies;
using Assignment.Application.Features.Deassign;
using AutoFixture;
using FluentValidation.TestHelper;
using Moq;
using static Assignment.Application.Validation.Errors;


namespace Assignment.Application.Tests.Features.Deassign;
public class DeassignValidatorTests : ApplicationTestBase
{
    [Fact]
    public async Task Validate_RoleNotFound_ReturnError()
    {
        var request = _fixture.Create<Application.Features.Deassign.Deassign>();

        var roleRepository = _fixture.Freeze<Mock<IRoleRepository>>();
        roleRepository
            .Setup(x => x.AnyAsync(request.RoleId, CancellationToken.None))
            .ReturnsAsync(false);

        var sut = _fixture.Create<DeassignValidator>();

        var result = await sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.RoleId);
        Assert.Equal(NOT_FOUND, result.Errors.Single().ErrorCode);
    }
}
