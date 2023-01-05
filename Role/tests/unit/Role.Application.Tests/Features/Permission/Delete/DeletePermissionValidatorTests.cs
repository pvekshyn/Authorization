using AutoFixture;
using FluentValidation.TestHelper;
using static Role.Application.Validation.Errors;
using Role.Application.Features.Permission.Delete;
using Moq;
using Role.Application.Dependencies;

namespace Role.Application.Tests.Features.Permission.Delete;
public class DeletePermissionValidatorTests : ApplicationTestBase
{
    [Fact]
    public async Task Validate_PermissionLinkedToRole_ReturnError()
    {
        var request = _fixture.Create<DeletePermission>();

        var permissionRepository = _fixture.Freeze<Mock<IPermissionRepository>>();
        permissionRepository
            .Setup(x => x.IsLinkedToAnyRole(request.PermissionId, CancellationToken.None))
            .ReturnsAsync(true);

        var sut = _fixture.Create<DeletePermissionValidator>();
        var result = await sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.PermissionId);
        Assert.Equal(LINKED_TO_ROLE, result.Errors.Single().ErrorCode);
    }
}
