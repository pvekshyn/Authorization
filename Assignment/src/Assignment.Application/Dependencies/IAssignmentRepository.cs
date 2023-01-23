namespace Assignment.Application.Dependencies
{
    public interface IAssignmentRepository
    {
        Task<bool> AnyAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);
        Task AssignAsync(Domain.Assignment assignment, CancellationToken cancellationToken);
        Task DeassignAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);
    }
}