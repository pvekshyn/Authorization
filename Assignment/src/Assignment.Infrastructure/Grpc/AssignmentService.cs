using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Infrastructure.Grpc
{
    public class AssignmentService : GrpcAssignmentService.GrpcAssignmentServiceBase
    {
        private readonly AssignmentDbContext _dbContext;
        public AssignmentService(AssignmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task GetAssignments(GrpcAssignmentsRequest request, IServerStreamWriter<GrpcAssignment> responseStream, ServerCallContext context)
        {
            var assignments = _dbContext.Assignments
                .AsNoTracking()
                .AsAsyncEnumerable();

            await foreach (var assignment in assignments)
            {
                await responseStream.WriteAsync(MapFrom(assignment));
            }
        }

        private GrpcAssignment MapFrom(Domain.Assignment Assignment)
        {
            return new GrpcAssignment()
            {
                Id = Assignment.Id.ToString(),
                UserId = Assignment.UserId.ToString(),
                RoleId = Assignment.RoleId.ToString()
            };
        }
    }
}