﻿using Assignment.SDK.DTO;

namespace Authorization.Application.Dependencies
{
    public interface IAssignmentRepository
    {
        void AddAssignment(AssignmentDto assignment);
        void DeleteAssignment(Guid id);
        void DeleteAssignments();
        void BulkInsertAssignments(IReadOnlyCollection<AssignmentDto> assignments);
    }
}
