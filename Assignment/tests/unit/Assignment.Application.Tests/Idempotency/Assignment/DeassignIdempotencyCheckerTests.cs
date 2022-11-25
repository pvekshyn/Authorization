using Assignment.Application.Dependencies;
using Assignment.Application.Features.Deassign;
using Assignment.Infrastructure;
using AutoFixture;

namespace Assignment.Application.Tests.Idempotency.Assignment;
public class DeassignIdempotencyCheckerTests : ApplicationTestBase
{
    [Fact]
    public async Task IsOperationAlreadyApplied_True()
    {
        using (var context = new AssignmentDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<Deassign>();

            _fixture.Inject<IAssignmentDbContext>(context);
            var sut = _fixture.Create<DeassignIdempotencyCheck>();

            var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

            Assert.True(result);
        }
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_False()
    {
        using (var context = new AssignmentDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<Deassign>();

            context.Assignments.Add(_fixture.CreateAssignment(request.UserId, request.RoleId));
            context.SaveChanges();

            _fixture.Inject<IAssignmentDbContext>(context);
            var sut = _fixture.Create<DeassignIdempotencyCheck>();

            var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

            Assert.False(result);
        }
    }
}
