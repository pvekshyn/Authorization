using Role.Domain;
using Role.Domain.ValueObjects.Permission;
using AutoFixture;
using Role.Domain.ValueObjects.Role;

namespace Role.Application.Tests;

public static class TestExtensions
{
    public static Permission CreatePermission(this IFixture fixture, Guid permissionId, string? permissionName = null)
    {
        if (permissionName == null)
        {
            permissionName = fixture.Create<string>();
        }

        return new Permission(
            new PermissionId(permissionId),
            new PermissionName(permissionName));
    }

    public static Domain.Role CreateRole(this IFixture fixture, Guid roleId, string? roleName = null)
    {
        if (roleName == null)
        {
            roleName = fixture.Create<string>().Substring(0, Constants.MaxRoleNameLength);
        }
        var permission = fixture.CreatePermission(Guid.NewGuid());

        return new Domain.Role(
            new RoleId(roleId),
            new RoleName(roleName),
            new List<Permission> { permission });
    }
}
