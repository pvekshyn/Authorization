using Role.Domain;
using Role.Infrastructure;
using Role.SDK.DTO;
using AutoFixture;
using FluentValidation.TestHelper;
using static Role.Application.Validation.Errors;
using Role.Application.Features.Role.CreateRole;

namespace Role.Application.Tests.Validation.Role;
public class CreateRoleValidatorTests : ApplicationTestBase
{
    [Fact]
    public async Task Validate_IdAlreadyExist_ReturnError()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var validator = new CreateRoleValidator(context);

            var dto = _fixture.Create<CreateRoleDto>();
            var request = new CreateRole(dto);

            foreach (var permissionId in dto.PermissionIds)
            {
                context.Permissions.Add(_fixture.CreatePermission(permissionId));
            }
            context.Roles.Add(_fixture.CreateRole(dto.Id));
            context.SaveChanges();

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.Role.Id);
            Assert.Equal(ALREADY_EXIST, result.Errors.Single().ErrorCode);
        }
    }

    [Fact]
    public async Task Validate_NameEmpty_ReturnError()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var validator = new CreateRoleValidator(context);

            var dto = _fixture.Build<CreateRoleDto>()
                .With(x => x.Name, string.Empty)
                .Create();

            var request = new CreateRole(dto);

            foreach (var permissionId in dto.PermissionIds)
            {
                context.Permissions.Add(_fixture.CreatePermission(permissionId));
            }
            context.SaveChanges();

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.Role.Name);
            Assert.Equal(EMPTY, result.Errors.Single().ErrorCode);
        }
    }

    [Fact]
    public async Task Validate_NameTooLong_ReturnError()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var validator = new CreateRoleValidator(context);

            var dto = _fixture.Build<CreateRoleDto>()
                .With(x => x.Name, new string('*', Constants.MaxRoleNameLength + 1))
                .Create();

            var request = new CreateRole(dto);

            foreach (var permissionId in dto.PermissionIds)
            {
                context.Permissions.Add(_fixture.CreatePermission(permissionId));
            }
            context.SaveChanges();

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.Role.Name);
            Assert.Equal(TOO_LONG, result.Errors.Single().ErrorCode);
        }
    }

    [Fact]
    public async Task Validate_NameAlreadyExist_ReturnError()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var validator = new CreateRoleValidator(context);

            var dto = _fixture.Create<CreateRoleDto>();
            var request = new CreateRole(dto);

            foreach (var permissionId in dto.PermissionIds)
            {
                context.Permissions.Add(_fixture.CreatePermission(permissionId));
            }

            context.Roles.Add(_fixture.CreateRole(
                _fixture.Create<Guid>(),
                dto.Name));
            context.SaveChanges();

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.Role.Name);
            Assert.Equal(ALREADY_EXIST, result.Errors.Single().ErrorCode);
        }
    }

    [Fact]
    public async Task Validate_PermissionIdsNotFound_ReturnError()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var validator = new CreateRoleValidator(context);

            var dto = _fixture.Create<CreateRoleDto>();
            var request = new CreateRole(dto);

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.Role.PermissionIds);
            Assert.Equal(NOT_FOUND, result.Errors.Single().ErrorCode);
        }
    }
}
