using Role.Domain;
using AutoFixture;
using FluentValidation.TestHelper;
using static Role.Application.Validation.Errors;
using Role.Application.Features.Role.CreateRole;

namespace Role.Application.Tests.Features.Role.Create;
public class CreateRoleValidatorTests : ApplicationTestBase
{
    private readonly CreateRoleValidator _sut;

    public CreateRoleValidatorTests()
    {
        _sut = _fixture.Create<CreateRoleValidator>();
    }

    [Fact]
    public async Task Validate_IdAlreadyExist_ReturnError()
    {
        var request = _fixture.Create<CreateRole>();

        var role = _fixture.CreateRole(request.Role.Id, request.Role.PermissionIds);
        _dbContext.Permissions.AddRange(role.Permissions);
        _dbContext.Roles.Add(role);
        _dbContext.SaveChanges();

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.Id);
        Assert.Equal(ALREADY_EXIST, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_NameEmpty_ReturnError()
    {
        var request = _fixture.Create<CreateRole>();
        request.Role.Name = string.Empty;

        var permissions = _fixture.CreatePermissions(request.Role.PermissionIds);
        _dbContext.Permissions.AddRange(permissions);
        _dbContext.SaveChanges();

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.Name);
        Assert.Equal(EMPTY, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_NameTooLong_ReturnError()
    {
        var request = _fixture.Create<CreateRole>();
        request.Role.Name = new string('*', Constants.MaxRoleNameLength + 1);

        var permissions = _fixture.CreatePermissions(request.Role.PermissionIds);
        _dbContext.Permissions.AddRange(permissions);
        _dbContext.SaveChanges();

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.Name);
        Assert.Equal(TOO_LONG, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_NameAlreadyExist_ReturnError()
    {
        var request = _fixture.Create<CreateRole>();

        var role = _fixture.CreateRole(_fixture.Create<Guid>(), request.Role.PermissionIds, request.Role.Name);
        _dbContext.Permissions.AddRange(role.Permissions);
        _dbContext.Roles.Add(role);
        _dbContext.SaveChanges();

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.Name);
        Assert.Equal(ALREADY_EXIST, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_PermissionIdsNotFound_ReturnError()
    {
        var request = _fixture.Create<CreateRole>();

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.PermissionIds);
        Assert.Equal(NOT_FOUND, result.Errors.Single().ErrorCode);
    }
}
