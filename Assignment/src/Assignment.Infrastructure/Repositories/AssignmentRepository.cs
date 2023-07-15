using Assignment.Application.Dependencies;
using Assignment.SDK.DTO;
using Assignment.SDK.Events;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Infrastructure.Repositories
{
    internal class AssignmentRepository : IAssignmentRepository
    {
        private readonly AssignmentDbContext _dbContext;
        private readonly ICapPublisher _capBus;

        public AssignmentRepository(AssignmentDbContext dbContext, ICapPublisher capBus)
        {
            _dbContext = dbContext;
            _capBus = capBus;
        }

        public async Task<bool> AnyAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
        {
            return await _dbContext.Assignments
                .AnyAsync(x => x.UserId == userId && x.RoleId == roleId, cancellationToken);
        }

        public async Task AssignAsync(Domain.Assignment assignment, CancellationToken cancellationToken)
        {
            var assignmentEvent = MapToAssignmentEvent(assignment);

            using (var trans = _dbContext.Database.BeginTransaction(_capBus, autoCommit: true))
            {
                await _dbContext.Assignments.AddAsync(assignment, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                _capBus.Publish(typeof(AssignmentEvent).FullName, assignmentEvent);
            }
        }

        public async Task DeassignAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
        {
            var assignment = await _dbContext.Assignments.SingleOrDefaultAsync(x =>
                        x.UserId == userId && x.RoleId == roleId, cancellationToken);

            if (assignment != null)
            {
                var deassignmentEvent = MapToDeassignmentEvent(assignment);

                using (var trans = _dbContext.Database.BeginTransaction(_capBus, autoCommit: true))
                {
                    _dbContext.Assignments.Remove(assignment);
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    _capBus.Publish(typeof(DeassignmentEvent).FullName, deassignmentEvent);
                }
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
