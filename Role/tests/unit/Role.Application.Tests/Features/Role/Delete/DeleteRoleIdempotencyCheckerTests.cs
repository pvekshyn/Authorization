using Role.Infrastructure;
using AutoFixture;
using Role.Application.Dependencies;
using Role.Application.Features.Role.DeleteRole;

namespace Role.Application.Tests.Features.Role.Delete;
public class DeleteRoleIdempotencyCheckerTests : ApplicationTestBase
{
    [Fact]
    public async Task IsOperationAlreadyApplied_True()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<DeleteRole>();

            _fixture.Inject<IRoleDbContext>(context);
            var sut = _fixture.Create<DeleteRoleIdempotencyCheck>();

            var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

            Assert.True(result);
        }
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_False()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<DeleteRole>();

            context.Roles.Add(_fixture.CreateRole(request.Id));
            context.SaveChanges();

            _fixture.Inject<IRoleDbContext>(context);
            var sut = _fixture.Create<DeleteRoleIdempotencyCheck>();

            var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

            Assert.False(result);
        }
    }
}
