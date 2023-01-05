using AutoFixture;
using FluentValidation.TestHelper;
using Moq;
using Role.Application.Dependencies;
using Role.Application.Features.Role.Delete;
using static Role.Application.Validation.Errors;

namespace Role.Application.Tests.Features.Role.Delete;
public class DeleteRoleValidatorTests : ApplicationTestBase
{
    [Fact]
    public async Task Validate_RoleExist_ReturnError()
    {
        var request = _fixture.Create<DeleteRole>();

        var roleRepository = _fixture.Freeze<Mock<IRoleRepository>>();
        roleRepository
            .Setup(x => x.AnyAsync(request.Id, CancellationToken.None))
            .ReturnsAsync(false);

        var sut = _fixture.Create<DeleteRoleValidator>();

        var result = await sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Id);
        Assert.Equal(NOT_FOUND, result.Errors.Single().ErrorCode);
    }
}
