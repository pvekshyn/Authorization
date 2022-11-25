using Assignment.Application.Dependencies;
using Assignment.Application.Features.Deassign;
using Assignment.Infrastructure;
using AutoFixture;

namespace Assignment.Application.Tests.Handlers.Assignment;

public class DeassignHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task Deassign_Success()
    {
        using (var context = new AssignmentDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<Deassign>();

            context.Assignments.Add(_fixture.CreateAssignment(request.UserId, request.RoleId));
            context.SaveChanges();

            _fixture.Inject<IAssignmentDbContext>(context);
            var sut = _fixture.Create<DeassignHandler>();

            var result = await sut.Handle(request, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Empty(context.Assignments);
        }
    }
}
