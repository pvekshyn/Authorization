﻿using Assignment.Application.Dependencies;
using Assignment.Application.Features.Assign;
using Assignment.Infrastructure;
using AutoFixture;

namespace Assignment.Application.Tests.Idempotency.Assignment;
public class AssignIdempotencyCheckerTests : ApplicationTestBase
{
    [Fact]
    public async Task IsOperationAlreadyApplied_True()
    {
        using (var context = new AssignmentDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<Assign>();

            context.Assignments.Add(_fixture.CreateAssignment(request.Assignment.UserId, request.Assignment.RoleId));
            context.SaveChanges();

            _fixture.Inject<IAssignmentDbContext>(context);
            var sut = _fixture.Create<AssignIdempotencyChecker>();

            var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

            Assert.True(result);
        }
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_False()
    {
        using (var context = new AssignmentDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<Assign>();

            _fixture.Inject<IAssignmentDbContext>(context);
            var sut = _fixture.Create<AssignIdempotencyChecker>();

            var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

            Assert.False(result);
        }
    }
}
