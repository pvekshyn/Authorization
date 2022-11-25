using Assignment.Application.Features.Deassign;
using Assignment.Infrastructure;
using AutoFixture;
using FluentValidation.TestHelper;
using static Assignment.Application.Validation.Errors;

namespace Assignment.Application.Tests.Validation.Assignment;
public class DeassignValidatorTests : ApplicationTestBase
{
    [Fact]
    public async Task Validate_RoleNotFound_ReturnError()
    {
        using (var context = new AssignmentDbContext(_dbContextOptions))
        {
            var validator = new DeassignValidator(context);

            var request = new Deassign(_fixture.Create<Guid>(), _fixture.Create<Guid>());

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.RoleId);
            Assert.Equal(NOT_FOUND, result.Errors.Single().ErrorCode);
        }
    }
}
