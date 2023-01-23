using Assignment.Domain.ValueObjects;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Infrastructure.Init
{
    public interface IDatabaseInitializer
    {
        Task InitRoles();
    }

    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly AssignmentDbContext _dbContext;
        private readonly GrpcRoleService.GrpcRoleServiceClient _grpcRoleClient;

        public DatabaseInitializer(AssignmentDbContext dbContext, GrpcRoleService.GrpcRoleServiceClient grpcRoleClient)
        {
            _dbContext = dbContext;
            _grpcRoleClient = grpcRoleClient;
        }

        public async Task InitRoles()
        {
            var anyRoleExist = await _dbContext.Roles.AnyAsync();
            if (!anyRoleExist)
            {
                var call = _grpcRoleClient.GetRoles(new GrpcRolesRequest { });
                await foreach (var grpcRole in call.ResponseStream.ReadAllAsync())
                {
                    _dbContext.Roles.Add(new Domain.Role(new RoleId(new Guid(grpcRole.Id))));
                }

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
