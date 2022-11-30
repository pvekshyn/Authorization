using Role.Infrastructure;
using AutoFixture;
using Role.Application.Dependencies;
using Role.Application.Features.Permission.DeletePermission;

namespace Role.Application.Tests.Features.Permission.Delete;

public class DeletePermissionHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task DeletePermission_Success()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<DeletePermission>();

            context.Permissions.Add(_fixture.CreatePermission(request.PermissionId));
            context.SaveChanges();

            _fixture.Inject<IRoleDbContext>(context);
            var sut = _fixture.Create<DeletePermissionHandler>();

            var result = await sut.Handle(request, CancellationToken.None);

            Assert.True(result.IsSuccess);

            Assert.Empty(context.Permissions);
        }
    }
}
