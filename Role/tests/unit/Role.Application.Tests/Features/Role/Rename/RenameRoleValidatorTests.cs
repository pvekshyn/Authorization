using Role.Domain;
using AutoFixture;
using FluentValidation.TestHelper;
using static Role.Application.Validation.Errors;
using Role.Application.Features.Role.RenameRole;

namespace Role.Application.Tests.Features.Role.Rename;
public class RenameRoleValidatorTests : ApplicationTestBase
{
    private readonly RenameRoleValidator _sut;

    public RenameRoleValidatorTests()
    {
        _sut = _fixture.Create<RenameRoleValidator>();
    }

    [Fact]
    public async Task Validate_RoleNotExist_ReturnError()
    {
        var request = _fixture.Create<RenameRole>();

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.Id);
        Assert.Equal(NOT_FOUND, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_NameEmpty_ReturnError()
    {
        var request = _fixture.Create<RenameRole>();
        request.Role.Name = string.Empty;

        _dbContext.Roles.Add(_fixture.CreateRole(request.Role.Id));
        _dbContext.SaveChanges();

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.Name);
        Assert.Equal(EMPTY, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_NameTooLong_ReturnError()
    {
        var request = _fixture.Create<RenameRole>();
        request.Role.Name = new string('*', Constants.MaxRoleNameLength + 1);

        _dbContext.Roles.Add(_fixture.CreateRole(request.Role.Id));
        _dbContext.SaveChanges();

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.Name);
        Assert.Equal(TOO_LONG, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_NameAlreadyExist_ReturnError()
    {
        var request = _fixture.Create<RenameRole>();

        _dbContext.Roles.Add(_fixture.CreateRole(request.Role.Id, roleName: request.Role.Name));
        _dbContext.SaveChanges();

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.Name);
        Assert.Equal(ALREADY_EXIST, result.Errors.Single().ErrorCode);
    }
}
