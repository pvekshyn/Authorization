using Assignment.Application.Dependencies;
using Assignment.Application.Features.Assign;
using Assignment.Infrastructure;
using AutoFixture;

namespace Assignment.Application.Tests.Handlers.Assignment;

public class AssignHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task Assign_Success()
    {
        using (var context = new AssignmentDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<Assign>();

            _fixture.Inject<IAssignmentDbContext>(context);
            var sut = _fixture.Create<AssignHandler>();

            var result = await sut.Handle(request, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(request.Assignment.Id, context.Assignments.Single().Id);
        }
    }
}
