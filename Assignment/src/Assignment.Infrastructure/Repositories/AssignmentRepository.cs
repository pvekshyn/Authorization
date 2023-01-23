using Assignment.Application.Dependencies;
using Assignment.SDK.DTO;
using Assignment.SDK.Events;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Infrastructure.Repositories
{
    internal class AssignmentRepository : IAssignmentRepository
    {
        private readonly AssignmentDbContext _dbContext;

        public AssignmentRepository(AssignmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AnyAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
        {
            return await _dbContext.Assignments
                .AnyAsync(x => x.UserId == userId && x.RoleId == roleId, cancellationToken);
        }

        public async Task AssignAsync(Domain.Assignment assignment, CancellationToken cancellationToken)
        {
            var assignmentEvent = MapToAssignmentEvent(assignment);

            await _dbContext.Assignments.AddAsync(assignment, cancellationToken);
            await _dbContext.AddPubSubOutboxMessageAsync(assignment.Id, assignmentEvent, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeassignAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
        {
            var assignment = await _dbContext.Assignments.SingleOrDefaultAsync(x =>
                        x.UserId == userId && x.RoleId == roleId, cancellationToken);

            if (assignment != null)
            {
                var deassignmentEvent = MapToDeassignmentEvent(assignment);

                _dbContext.Assignments.Remove(assignment);
                await _dbContext.AddPubSubOutboxMessageAsync(assignment.Id, deassignmentEvent, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        private static AssignmentEvent MapToAssignmentEvent(Domain.Assignment assignment)
        {
            return new AssignmentEvent
            {
                Assignment = new AssignmentDto
                {
                    Id = assignment.Id,
                    UserId = assignment.UserId,
                    RoleId = assignment.RoleId
                }
            };
        }

        private static DeassignmentEvent MapToDeassignmentEvent(Domain.Assignment assignment)
        {
            return new DeassignmentEvent
            {
                Assignment = new AssignmentDto
                {
                    Id = assignment.Id,
                    UserId = assignment.UserId,
                    RoleId = assignment.RoleId
                }
            };
        }
    }
}
