using Assignment.Application.Dependencies;
using Assignment.Application.Features.Assign;
using AutoFixture;
using Moq;

namespace Assignment.Application.Tests.Features.Assign;
public class AssignIdempotencyCheckerTests : ApplicationTestBase
{
    [Fact]
    public async Task IsOperationAlreadyApplied_True()
    {
        var request = _fixture.Create<Application.Features.Assign.Assign>();

        var assignmentRepository = _fixture.Freeze<Mock<IAssignmentRepository>>();
        assignmentRepository
            .Setup(x => x.AnyAsync(request.Assignment.UserId, request.Assignment.RoleId, CancellationToken.None))
            .ReturnsAsync(true);

        var _sut = _fixture.Create<AssignIdempotencyCheck>();

        var result = await _sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_False()
    {
        var request = _fixture.Create<Application.Features.Assign.Assign>();

        var assignmentRepository = _fixture.Freeze<Mock<IAssignmentRepository>>();
        assignmentRepository
            .Setup(x => x.AnyAsync(request.Assignment.UserId, request.Assignment.RoleId, CancellationToken.None))
            .ReturnsAsync(false);

        var _sut = _fixture.Create<AssignIdempotencyCheck>();

        var result = await _sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.False(result);
    }
}
