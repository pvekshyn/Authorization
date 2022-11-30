using AutoFixture;
using FluentValidation.TestHelper;
using static Role.Application.Validation.Errors;
using Role.Application.Features.Role.UpdateRolePermissions;

namespace Role.Application.Tests.Features.Role.UpdatePermissions;
public class UpdateRolePermissionValidatorTests : ApplicationTestBase
{
    private readonly UpdateRolePermissionsValidator _sut;

    public UpdateRolePermissionValidatorTests()
    {
        _sut = _fixture.Create<UpdateRolePermissionsValidator>();
    }

    [Fact]
    public async Task Validate_RoleNotExist_ReturnError()
    {
        var request = _fixture.Create<UpdateRolePermissions>();

        _dbContext.Permissions.AddRange(_fixture.CreatePermissions(request.Role.PermissionIds));
        _dbContext.SaveChanges();

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.Id);
        Assert.Equal(NOT_FOUND, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_PermissionIdsEmpty_ReturnError()
    {
        var request = _fixture.Create<UpdateRolePermissions>();
        request.Role.PermissionIds = Array.Empty<Guid>();

        _dbContext.Roles.Add(_fixture.CreateRole(request.Role.Id));
        _dbContext.SaveChanges();

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.PermissionIds);
        Assert.Equal(EMPTY, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_PermissionIdsNotFound_ReturnError()
    {
        var request = _fixture.Create<UpdateRolePermissions>();

        _dbContext.Roles.Add(_fixture.CreateRole(request.Role.Id));
        _dbContext.SaveChanges();

        var result = await _sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.PermissionIds);
        Assert.Equal(NOT_FOUND, result.Errors.Single().ErrorCode);
    }
}
