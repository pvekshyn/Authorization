using Role.Infrastructure;
using AutoFixture;
using Role.Application.Dependencies;
using Role.Application.Features.Permission.CreatePermission;

namespace Role.Application.Tests.Handlers.Permission;
public class CreatePermissionHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task CreatePermission_Success()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<CreatePermission>();

            _fixture.Inject<IRoleDbContext>(context);
            var sut = _fixture.Create<CreatePermissionHandler>();

            var result = await sut.Handle(request, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }
    }
}
