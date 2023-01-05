using Role.Domain;
using AutoFixture;
using FluentValidation.TestHelper;
using static Role.Application.Validation.Errors;
using Role.Application.Features.Permission.Create;
using Moq;
using Role.Application.Dependencies;

namespace Role.Application.Tests.Features.Permission.Create;
public class CreatePermissionValidatorTests : ApplicationTestBase
{
    public CreatePermissionValidatorTests()
    {
    }

    [Fact]
    public async Task Validate_IdAlreadyExist_ReturnError()
    {
        var request = _fixture.Create<CreatePermission>();

        var permissionRepository = _fixture.Freeze<Mock<IPermissionRepository>>();
        permissionRepository
            .Setup(x => x.AnyAsync(request.Permission.Id, CancellationToken.None))
            .ReturnsAsync(true);

        var _sut = _fixture.Create<CreatePermissionValidator>();

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Permission.Id);
        Assert.Equal(ALREADY_EXIST, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_NameEmpty_ReturnError()
    {
        var request = _fixture.Create<CreatePermission>();
        request.Permission.Name = string.Empty;

        var permissionRepository = _fixture.Freeze<Mock<IPermissionRepository>>();
        permissionRepository
            .Setup(x => x.AnyAsync(request.Permission.Id, CancellationToken.None))
            .ReturnsAsync(false);

        var _sut = _fixture.Create<CreatePermissionValidator>();

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Permission.Name);
        Assert.Equal(EMPTY, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_NameTooLong_ReturnError()
    {
        var request = _fixture.Create<CreatePermission>();
        request.Permission.Name = new string('*', Constants.MaxPermissionNameLength + 1);

        var permissionRepository = _fixture.Freeze<Mock<IPermissionRepository>>();
        permissionRepository
            .Setup(x => x.AnyAsync(request.Permission.Id, CancellationToken.None))
            .ReturnsAsync(false);

        var _sut = _fixture.Create<CreatePermissionValidator>();

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Permission.Name);
        Assert.Equal(TOO_LONG, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_NameAlreadyExist_ReturnError()
    {
        var request = _fixture.Create<CreatePermission>();

        var permissionRepository = _fixture.Freeze<Mock<IPermissionRepository>>();
        permissionRepository
            .Setup(x => x.AnyAsync(request.Permission.Id, CancellationToken.None))
            .ReturnsAsync(false);

        permissionRepository
            .Setup(x => x.AnyAsync(request.Permission.Name, CancellationToken.None))
            .ReturnsAsync(true);

        var _sut = _fixture.Create<CreatePermissionValidator>();

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Permission.Name);
        Assert.Equal(ALREADY_EXIST, result.Errors.Single().ErrorCode);
    }
}
