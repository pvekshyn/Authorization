using Role.Domain;
using AutoFixture;
using FluentValidation.TestHelper;
using static Role.Application.Validation.Errors;
using Role.Application.Features.Role.Create;
using Moq;
using Role.Application.Dependencies;

namespace Role.Application.Tests.Features.Role.Create;
public class CreateRoleValidatorTests : ApplicationTestBase
{
    [Fact]
    public async Task Validate_IdAlreadyExist_ReturnError()
    {
        var request = _fixture.Create<CreateRole>();

        var roleRepository = _fixture.Freeze<Mock<IRoleRepository>>();
        roleRepository
            .Setup(x => x.AnyAsync(request.Role.Id, CancellationToken.None))
            .ReturnsAsync(true);

        var sut = _fixture.Create<CreateRoleValidator>();

        var result = await sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.Id);
        Assert.Equal(ALREADY_EXIST, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_NameEmpty_ReturnError()
    {
        var request = _fixture.Create<CreateRole>();
        request.Role.Name = string.Empty;

        var roleRepository = _fixture.Freeze<Mock<IRoleRepository>>();
        roleRepository
            .Setup(x => x.AnyAsync(request.Role.Id, CancellationToken.None))
            .ReturnsAsync(false);

        var sut = _fixture.Create<CreateRoleValidator>();

        var result = await sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.Name);
        Assert.Equal(EMPTY, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_NameTooLong_ReturnError()
    {
        var request = _fixture.Create<CreateRole>();
        request.Role.Name = new string('*', Constants.MaxRoleNameLength + 1);
        var roleRepository = _fixture.Freeze<Mock<IRoleRepository>>();
        roleRepository
            .Setup(x => x.AnyAsync(request.Role.Id, CancellationToken.None))
            .ReturnsAsync(false);

        var sut = _fixture.Create<CreateRoleValidator>();

        var result = await sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.Name);
        Assert.Equal(TOO_LONG, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_NameAlreadyExist_ReturnError()
    {
        var request = _fixture.Create<CreateRole>();

        var roleRepository = _fixture.Freeze<Mock<IRoleRepository>>();
        roleRepository
            .Setup(x => x.AnyAsync(request.Role.Id, CancellationToken.None))
            .ReturnsAsync(false);

        roleRepository
            .Setup(x => x.AnyAsync(request.Role.Name, CancellationToken.None))
            .ReturnsAsync(true);

        var sut = _fixture.Create<CreateRoleValidator>();

        var result = await sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.Name);
        Assert.Equal(ALREADY_EXIST, result.Errors.Single().ErrorCode);
    }

    [Fact]
    public async Task Validate_PermissionIdsNotFound_ReturnError()
    {
        var request = _fixture.Create<CreateRole>();

        var roleRepository = _fixture.Freeze<Mock<IRoleRepository>>();
        roleRepository
            .Setup(x => x.AnyAsync(request.Role.Id, CancellationToken.None))
            .ReturnsAsync(false);
        roleRepository
            .Setup(x => x.AnyAsync(request.Role.Name, CancellationToken.None))
            .ReturnsAsync(false);

        var permissionRepository = _fixture.Freeze<Mock<IPermissionRepository>>();
        permissionRepository
            .Setup(x => x.GetAsync(It.IsAny<IReadOnlyCollection<Guid>>(), CancellationToken.None))
            .ReturnsAsync(new List<Domain.Permission>());

        var sut = _fixture.Create<CreateRoleValidator>();

        var result = await sut.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Role.PermissionIds);
        Assert.Equal(NOT_FOUND, result.Errors.Single().ErrorCode);
    }
}
