using Role.Domain;
using AutoFixture;
using FluentValidation.TestHelper;
using static Role.Application.Validation.Errors;
using Role.Application.Features.Role.Rename;
using Moq;
using Role.Application.Dependencies;

namespace Role.Application.Tests.Features.Role.Rename;
public class RenameRoleValidatorTests : ApplicationTestBase
{
    [Fact]
    public async Task Validate_RoleNotExist_ReturnError()
    {
        var request = _fixture.Create<RenameRole>();

        var roleRepository = _fixture.Freeze<Mock<IRoleRepository>>();
        roleRepository
            .Setup(x => x.AnyAsync(request.Role.Id, CancellationToken.None))
            .ReturnsAsync(false);

        var sut = _fixture.Create<RenameRoleValidator>();

        var result = await sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.Id);
        Assert.Equal(NOT_FOUND, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_NameEmpty_ReturnError()
    {
        var request = _fixture.Create<RenameRole>();
        request.Role.Name = string.Empty;

        var sut = _fixture.Create<RenameRoleValidator>();

        var result = await sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.Name);
        Assert.Equal(EMPTY, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_NameTooLong_ReturnError()
    {
        var request = _fixture.Create<RenameRole>();
        request.Role.Name = new string('*', Constants.MaxRoleNameLength + 1);

        var sut = _fixture.Create<RenameRoleValidator>();

        var result = await sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.Name);
        Assert.Equal(TOO_LONG, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_NameAlreadyExist_ReturnError()
    {
        var request = _fixture.Create<RenameRole>();

        var roleRepository = _fixture.Freeze<Mock<IRoleRepository>>();
        roleRepository
            .Setup(x => x.AnyAsync(request.Role.Name, CancellationToken.None))
            .ReturnsAsync(true);

        var sut = _fixture.Create<RenameRoleValidator>();

        var result = await sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.Name);
        Assert.Equal(ALREADY_EXIST, result.Errors.Single().ErrorCode);
    }
}
