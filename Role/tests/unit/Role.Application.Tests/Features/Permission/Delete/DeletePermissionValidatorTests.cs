using Role.Infrastructure;
using AutoFixture;
using FluentValidation.TestHelper;
using static Role.Application.Validation.Errors;
using Role.Application.Features.Permission.DeletePermission;

namespace Role.Application.Tests.Features.Permission.Delete;
public class DeletePermissionValidatorTests : ApplicationTestBase
{
    [Fact]
    public async Task Validate_IdAlreadyExist_ReturnError()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var validator = new DeletePermissionValidator(context);

            var permissionId = _fixture.Create<Guid>();
            var request = new DeletePermission(permissionId);

            var permission = _fixture.CreatePermission(permissionId);
            var role = _fixture.CreateRole(Guid.NewGuid());
            role.AddPermission(permission);
            context.Roles.Add(role);
            context.SaveChanges();

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.PermissionId);
            Assert.Equal(LINKED_TO_ROLE, result.Errors.Single().ErrorCode);
        }
    }
}
