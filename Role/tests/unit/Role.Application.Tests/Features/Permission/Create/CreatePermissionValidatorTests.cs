using Role.Domain;
using AutoFixture;
using FluentValidation.TestHelper;
using static Role.Application.Validation.Errors;
using Role.Application.Features.Permission.CreatePermission;

namespace Role.Application.Tests.Features.Permission.Create;
public class CreatePermissionValidatorTests : ApplicationTestBase
{
    private readonly CreatePermissionValidator _sut;

    public CreatePermissionValidatorTests()
    {
        _sut = _fixture.Create<CreatePermissionValidator>();
    }

    [Fact]
    public async Task Validate_IdAlreadyExist_ReturnError()
    {
        var request = _fixture.Create<CreatePermission>();

        _dbContext.Permissions.Add(_fixture.CreatePermission(request.Permission.Id));
        _dbContext.SaveChanges();

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Permission.Id);
        Assert.Equal(ALREADY_EXIST, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_NameEmpty_ReturnError()
    {
        var request = _fixture.Create<CreatePermission>();
        request.Permission.Name = string.Empty;

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Permission.Name);
        Assert.Equal(EMPTY, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_NameTooLong_ReturnError()
    {
        var request = _fixture.Create<CreatePermission>();
        request.Permission.Name = new string('*', Constants.MaxPermissionNameLength + 1);

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Permission.Name);
        Assert.Equal(TOO_LONG, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_NameAlreadyExist_ReturnError()
    {
        var request = _fixture.Create<CreatePermission>();

        _dbContext.Permissions.Add(_fixture.CreatePermission(_fixture.Create<Guid>(), request.Permission.Name));
        _dbContext.SaveChanges();

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Permission.Name);
        Assert.Equal(ALREADY_EXIST, result.Errors.Single().ErrorCode);
    }
}
