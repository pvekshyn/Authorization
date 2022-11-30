using Role.Infrastructure;
using AutoFixture;
using Role.Application.Dependencies;
using Role.Application.Features.Permission.DeletePermission;

namespace Role.Application.Tests.Features.Permission.Delete;
public class DeletePermissionIdempotencyCheckerTests : ApplicationTestBase
{
    [Fact]
    public async Task IsOperationAlreadyApplied_True()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<DeletePermission>();

            _fixture.Inject<IRoleDbContext>(context);
            var sut = _fixture.Create<DeletePermissionIdempotencyCheck>();

            var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

            Assert.True(result);
        }
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_False()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<DeletePermission>();

            context.Permissions.Add(_fixture.CreatePermission(request.PermissionId));
            context.SaveChanges();

            _fixture.Inject<IRoleDbContext>(context);
            var sut = _fixture.Create<DeletePermissionIdempotencyCheck>();

            var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

            Assert.False(result);
        }
    }
}
