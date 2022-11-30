using Role.Domain;
using Role.Infrastructure;
using Role.SDK.DTO;
using AutoFixture;
using FluentValidation.TestHelper;
using static Role.Application.Validation.Errors;
using Role.Application.Features.Role.RenameRole;

namespace Role.Application.Tests.Features.Role.Rename;
public class RenameRoleValidatorTests : ApplicationTestBase
{
    [Fact]
    public async Task Validate_RoleNotExist_ReturnError()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var validator = new RenameRoleValidator(context);

            var request = _fixture.Create<RenameRole>();

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.Role.Id);
            Assert.Equal(NOT_FOUND, result.Errors.Single().ErrorCode);
        }
    }

    [Fact]
    public async Task Validate_NameEmpty_ReturnError()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var validator = new RenameRoleValidator(context);

            var dto = _fixture.Build<RenameRoleDto>()
                .With(x => x.Name, string.Empty)
                .Create();

            var request = new RenameRole(dto);

            context.Roles.Add(_fixture.CreateRole(dto.Id));
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
            var validator = new RenameRoleValidator(context);

            var dto = _fixture.Build<RenameRoleDto>()
                .With(x => x.Name, new string('*', Constants.MaxRoleNameLength + 1))
                .Create();

            var request = new RenameRole(dto);

            context.Roles.Add(_fixture.CreateRole(dto.Id));
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
            var validator = new RenameRoleValidator(context);

            var dto = _fixture.Create<RenameRoleDto>();
            var request = new RenameRole(dto);

            context.Roles.Add(_fixture.CreateRole(dto.Id, dto.Name));
            context.SaveChanges();

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.Role.Name);
            Assert.Equal(ALREADY_EXIST, result.Errors.Single().ErrorCode);
        }
    }
}
