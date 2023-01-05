using AutoFixture;
using FluentValidation.TestHelper;
using static Role.Application.Validation.Errors;
using Role.Application.Features.Role.UpdatePermissions;
using Moq;
using Role.Application.Dependencies;

namespace Role.Application.Tests.Features.Role.UpdatePermissions;
public class UpdateRolePermissionValidatorTests : ApplicationTestBase
{
    [Fact]
    public async Task Validate_RoleNotExist_ReturnError()
    {
        var request = _fixture.Create<UpdateRolePermissions>();

        var roleRepository = _fixture.Freeze<Mock<IRoleRepository>>();
        roleRepository
            .Setup(x => x.AnyAsync(request.Role.Id, CancellationToken.None))
            .ReturnsAsync(false);

        var sut = _fixture.Create<UpdateRolePermissionsValidator>();
        var result = await sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.Id);
        Assert.Equal(NOT_FOUND, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_PermissionIdsEmpty_ReturnError()
    {
        var request = _fixture.Create<UpdateRolePermissions>();
        request.Role.PermissionIds = Array.Empty<Guid>();

        var sut = _fixture.Create<UpdateRolePermissionsValidator>();

        var result = await sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.PermissionIds);
        Assert.Equal(EMPTY, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_PermissionIdsNotFound_ReturnError()
    {
        var request = _fixture.Create<UpdateRolePermissions>();

        var permissionRepository = _fixture.Freeze<Mock<IPermissionRepository>>();
        permissionRepository
            .Setup(x => x.GetAsync(It.IsAny<IReadOnlyCollection<Guid>>(), CancellationToken.None))
            .ReturnsAsync(new List<Domain.Permission>());

        var sut = _fixture.Create<UpdateRolePermissionsValidator>();

        var result = await sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.PermissionIds);
        Assert.Equal(NOT_FOUND, result.Errors.Single().ErrorCode);
    }
}
