using Assignment.Application.Dependencies;
using Assignment.Application.Features.Assign;
using AutoFixture;
using FluentValidation.TestHelper;
using Moq;
using static Assignment.Application.Validation.Errors;

namespace Assignment.Application.Tests.Features.Assign;
public class AssignValidatorTests : ApplicationTestBase
{
    [Fact]
    public async Task Validate_RoleNotFound_ReturnError()
    {
        var request = _fixture.Create<Application.Features.Assign.Assign>();

        var roleRepository = _fixture.Freeze<Mock<IRoleRepository>>();
        roleRepository
            .Setup(x => x.AnyAsync(request.Assignment.RoleId, CancellationToken.None))
            .ReturnsAsync(false);

        var sut = _fixture.Create<AssignValidator>();

        var result = await sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Assignment.RoleId);
        Assert.Equal(NOT_FOUND, result.Errors.Single().ErrorCode);
    }
}
