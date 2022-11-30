using AutoFixture;
using FluentValidation.TestHelper;
using static Role.Application.Validation.Errors;
using Role.Application.Features.Permission.DeletePermission;

namespace Role.Application.Tests.Features.Permission.Delete;
public class DeletePermissionValidatorTests : ApplicationTestBase
{
    [Fact]
    public async Task Validate_PermissionLinkedToRole_ReturnError()
    {
        var request = _fixture.Create<DeletePermission>();

        var role = _fixture.CreateRole(Guid.NewGuid(), permissionIds: new List<Guid> { request.PermissionId });
        _dbContext.Roles.Add(role);
        _dbContext.SaveChanges();

        var sut = _fixture.Create<DeletePermissionValidator>();
        var result = await sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.PermissionId);
        Assert.Equal(LINKED_TO_ROLE, result.Errors.Single().ErrorCode);
    }
}
