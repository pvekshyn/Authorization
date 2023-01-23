using Assignment.Application.Dependencies;
using Assignment.Application.Features.Deassign;
using AutoFixture;
using Moq;

namespace Assignment.Application.Tests.Features.Deassign;
public class DeassignIdempotencyCheckTests : ApplicationTestBase
{
    [Fact]
    public async Task IsOperationAlreadyApplied_True()
    {
        var request = _fixture.Create<Application.Features.Deassign.Deassign>();

        var assignmentRepository = _fixture.Freeze<Mock<IAssignmentRepository>>();
        assignmentRepository
            .Setup(x => x.AnyAsync(request.UserId, request.RoleId, CancellationToken.None))
            .ReturnsAsync(false);

        var _sut = _fixture.Create<DeassignIdempotencyCheck>();

        var result = await _sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_False()
    {
        var request = _fixture.Create<Application.Features.Deassign.Deassign>();

        var assignmentRepository = _fixture.Freeze<Mock<IAssignmentRepository>>();
        assignmentRepository
            .Setup(x => x.AnyAsync(request.UserId, request.RoleId, CancellationToken.None))
            .ReturnsAsync(true);

        var _sut = _fixture.Create<DeassignIdempotencyCheck>();

        var result = await _sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

        Assert.False(result);
    }
}
