using Assignment.SDK.DTO;
using Common.SDK;
using Refit;

namespace Assignment.SDK.Features
{
    [Headers("Authorization: Bearer")]
    public interface IAssignmentApi
    {
        [Post("/assign")]
        Task<Result> AssignAsync(AssignmentDto assignmentDto);

        [Post("/deassign")]
        Task<Result> DeassignAsync(Guid userId, Guid roleId);
    }
}
