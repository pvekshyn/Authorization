using AutoFixture;
using FluentValidation.TestHelper;
using Role.Application.Features.Role.DeleteRole;
using Role.Infrastructure;
using static Role.Application.Validation.Errors;

namespace Role.Application.Tests.Features.Role.Delete;
public class DeleteRoleValidatorTests : ApplicationTestBase
{
    [Fact]
    public async Task Validate_RoleExist_ReturnError()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var validator = new DeleteRoleValidator(context);

            var roleId = _fixture.Create<Guid>();
            var request = new DeleteRole(roleId);

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.Id);
            Assert.Equal(NOT_FOUND, result.Errors.Single().ErrorCode);
        }
    }
}
