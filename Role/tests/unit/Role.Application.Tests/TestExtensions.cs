using Role.Domain;
using Role.Domain.ValueObjects.Permission;
using AutoFixture;
using Role.Domain.ValueObjects.Role;

namespace Role.Application.Tests;

public static class TestExtensions
{
    public static Permission CreatePermission(this IFixture fixture, Guid id, string? name = null)
    {
        if (name == null)
        {
            name = fixture.Create<string>();
        }

        return new Permission(
            new PermissionId(id),
            new PermissionName(name));
    }

    public static IReadOnlyCollection<Permission> CreatePermissions(this IFixture fixture, IReadOnlyCollection<Guid> permissionIds)
    {
        return permissionIds.Select(x => fixture.CreatePermission(x)).ToList();
    }

    public static Domain.Role CreateRole(this IFixture fixture, Guid roleId, IReadOnlyCollection<Guid> permissionIds = null, string? roleName = null)
    {
        if (roleName == null)
        {
            roleName = fixture.Create<string>().Substring(0, Constants.MaxRoleNameLength);
        }

        var permissions = permissionIds?.Select(x => fixture.CreatePermission(x)).ToList()
            ?? new List<Permission> { fixture.CreatePermission(Guid.NewGuid()) };

        return new Domain.Role(
            new RoleId(roleId),
            new RoleName(roleName),
            permissions);
    }
}
