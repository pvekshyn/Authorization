using Assignment.SDK.DTO;
using Authorization.Application.Dependencies;
using Grpc.Core;

namespace Authorization.Infrastructure.Init
{
    public interface IDatabaseInitializer
    {
        Task Reset();
    }

    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly GrpcRoleService.GrpcRoleServiceClient _grpcRoleClient;
        private readonly GrpcAssignmentService.GrpcAssignmentServiceClient _grpcAssignmentClient;

        public DatabaseInitializer(
            IRolePermissionRepository repository,
            IAssignmentRepository assignmentRepository,
            GrpcRoleService.GrpcRoleServiceClient grpcRoleClient,
            GrpcAssignmentService.GrpcAssignmentServiceClient grpcAssignmentClient)
        {
            _rolePermissionRepository = repository;
            _assignmentRepository = assignmentRepository;
            _grpcRoleClient = grpcRoleClient;
            _grpcAssignmentClient = grpcAssignmentClient;
        }

        public async Task Reset()
        {
            await InitRolePermissionsGrpcAsync();
            await InitAssignmentsGrpcAsync();
        }

        private async Task InitRolePermissionsGrpcAsync()
        {
            var rolesCall = _grpcRoleClient.GetRoles(new GrpcRolesRequest { });

            var roles = new List<GrpcRole>();
            await foreach (var grpcRole in rolesCall.ResponseStream.ReadAllAsync())
            {
                roles.Add(grpcRole);
            }

            var permissionsCall = _grpcRoleClient.GetPermissions(new GrpcPermissionsRequest { });

            var permissions = new List<GrpcPermission>();
            await foreach (var grpcPermission in permissionsCall.ResponseStream.ReadAllAsync())
            {
                permissions.Add(grpcPermission);
            }

            _rolePermissionRepository.DeleteRoles();
            _rolePermissionRepository.DeletePermissions();

            var permissionTuples = permissions.Select(x => (new Guid(x.Id), x.Name)).ToList();
            _rolePermissionRepository.BulkInsertPermissions(permissionTuples);

            var roleTuples = roles.Select(x => (new Guid(x.Id), x.Name)).ToList();
            _rolePermissionRepository.BulkInsertRoles(roleTuples);

            var rolepermissions = roles
                .SelectMany<GrpcRole, GrpcPermission, (Guid roleId, Guid permissionId)>(x => x.Permissions, (role, permission) => new(new Guid(role.Id), new Guid(permission.Id))).ToList();
            _rolePermissionRepository.BulkInsertRolePermissions(rolepermissions);
        }

        private async Task InitAssignmentsGrpcAsync()
        {
            var call = _grpcAssignmentClient.GetAssignments(new GrpcAssignmentsRequest { });

            var assignments = new List<AssignmentDto>();
            await foreach (var grpcAssignment in call.ResponseStream.ReadAllAsync())
            {
                assignments.Add(
                new AssignmentDto
                {
                    Id = new Guid(grpcAssignment.Id),
                    UserId = new Guid(grpcAssignment.UserId),
                    RoleId = new Guid(grpcAssignment.RoleId)
                });
            }

            _assignmentRepository.DeleteAssignments();

            _assignmentRepository.BulkInsertAssignments(assignments);
        }
    }
}
