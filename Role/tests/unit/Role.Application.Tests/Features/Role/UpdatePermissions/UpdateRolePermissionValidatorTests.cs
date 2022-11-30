using Role.Infrastructure;
using Role.SDK.DTO;
using AutoFixture;
using FluentValidation.TestHelper;
using static Role.Application.Validation.Errors;
using Role.Application.Features.Role.UpdateRolePermissions;

namespace Role.Application.Tests.Features.Role.UpdatePermissions;
public class UpdateRolePermissionValidatorTests : ApplicationTestBase
{
    [Fact]
    public async Task Validate_RoleNotExist_ReturnError()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var validator = new UpdateRolePermissionsValidator(context);

            var dto = _fixture.Create<UpdateRolePermissionsDto>();
            var request = new UpdateRolePermissions(dto);

            foreach (var permissionId in dto.PermissionIds)
            {
                context.Permissions.Add(_fixture.CreatePermission(permissionId));
            }
            context.SaveChanges();

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.Role.Id);
            Assert.Equal(NOT_FOUND, result.Errors.Single().ErrorCode);
        }
    }

    [Fact]
    public async Task Validate_PermissionIdsEmpty_ReturnError()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var validator = new UpdateRolePermissionsValidator(context);

            var dto = _fixture.Build<UpdateRolePermissionsDto>()
                .With(x => x.PermissionIds, Array.Empty<Guid>())
                .Create();
            var request = new UpdateRolePermissions(dto);

            context.Roles.Add(_fixture.CreateRole(dto.Id));
            context.SaveChanges();

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.Role.PermissionIds);
            Assert.Equal(EMPTY, result.Errors.Single().ErrorCode);
        }
    }

    [Fact]
    public async Task Validate_PermissionIdsNotFound_ReturnError()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var validator = new UpdateRolePermissionsValidator(context);

            var dto = _fixture.Create<UpdateRolePermissionsDto>();
            var request = new UpdateRolePermissions(dto);

            context.Roles.Add(_fixture.CreateRole(dto.Id));
            context.SaveChanges();

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.Role.PermissionIds);
            Assert.Equal(NOT_FOUND, result.Errors.Single().ErrorCode);
        }
    }
}
