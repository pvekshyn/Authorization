using Role.Domain;
using Role.Infrastructure;
using Role.SDK.DTO;
using AutoFixture;
using FluentValidation.TestHelper;
using static Role.Application.Validation.Errors;
using Role.Application.Features.Permission.CreatePermission;

namespace Role.Application.Tests.Validation.Permission;
public class CreatePermissionValidatorTests : ApplicationTestBase
{
    [Fact]
    public async Task Validate_IdAlreadyExist_ReturnError()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var validator = new CreatePermissionValidator(context);

            var dto = _fixture.Create<PermissionDto>();
            var request = new CreatePermission(dto);

            context.Permissions.Add(_fixture.CreatePermission(dto.Id));
            context.SaveChanges();

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.Permission.Id);
            Assert.Equal(ALREADY_EXIST, result.Errors.Single().ErrorCode);
        }
    }

    [Fact]
    public async Task Validate_NameEmpty_ReturnError()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var validator = new CreatePermissionValidator(context);

            var dto = new PermissionDto { Name = string.Empty };
            var request = new CreatePermission(dto);

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.Permission.Name);
            Assert.Equal(EMPTY, result.Errors.Single().ErrorCode);
        }
    }

    [Fact]
    public async Task Validate_NameTooLong_ReturnError()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var validator = new CreatePermissionValidator(context);

            var dto = new PermissionDto { Name = new string('*', Constants.MaxPermissionNameLength + 1) };
            var request = new CreatePermission(dto);

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.Permission.Name);
            Assert.Equal(TOO_LONG, result.Errors.Single().ErrorCode);
        }
    }

    [Fact]
    public async Task Validate_NameAlreadyExist_ReturnError()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var validator = new CreatePermissionValidator(context);

            var dto = _fixture.Create<PermissionDto>();
            var request = new CreatePermission(dto);

            context.Permissions.Add(_fixture.CreatePermission(
                _fixture.Create<Guid>(),
                dto.Name));
            context.SaveChanges();

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.Permission.Name);
            Assert.Equal(ALREADY_EXIST, result.Errors.Single().ErrorCode);
        }
    }
}
