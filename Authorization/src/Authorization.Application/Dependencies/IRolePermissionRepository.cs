using Role.SDK.DTO;

namespace Authorization.Application.Dependencies
{
    public interface IRolePermissionRepository
    {
        void AddPermission(PermissionDto permission);
        void DeletePermission(Guid id);
        void AddRole(CreateRoleDto role);
        void RenameRole(RenameRoleDto role);
        void UpdateRolePermissions(UpdateRolePermissionsDto role);
        void DeleteRole(Guid id);
        void DeleteRoles();
        void DeletePermissions();
        void BulkInsertRoles(IReadOnlyCollection<(Guid Id, string Name)> roles);
        void BulkInsertRolePermissions(IReadOnlyCollection<(Guid roleId, Guid permissionId)> rolePermissions);
        void BulkInsertPermissions(IReadOnlyCollection<(Guid Id, string Name)> permissions);
    }
}
