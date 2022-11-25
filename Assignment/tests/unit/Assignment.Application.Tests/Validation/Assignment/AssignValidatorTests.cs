using Assignment.Application.Features.Assign;
using Assignment.Infrastructure;
using Assignment.SDK.DTO;
using AutoFixture;
using FluentValidation.TestHelper;
using static Assignment.Application.Validation.Errors;

namespace Assignment.Application.Tests.Validation.Assignment;
public class AssignValidatorTests : ApplicationTestBase
{
    [Fact]
    public async Task Validate_RoleNotFound_ReturnError()
    {
        using (var context = new AssignmentDbContext(_dbContextOptions))
        {
            var validator = new AssignValidator(context);

            var dto = _fixture.Create<AssignmentDto>();
            var request = new Assign(dto);

            var result = await validator.TestValidateAsync(request);

            result.ShouldHaveValidationErrorFor(x => x.Assignment.RoleId);
            Assert.Equal(NOT_FOUND, result.Errors.Single().ErrorCode);
        }
    }
}
