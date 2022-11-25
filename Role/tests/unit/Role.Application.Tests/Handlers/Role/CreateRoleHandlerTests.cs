using Role.Infrastructure;
using AutoFixture;
using Role.Application.Dependencies;
using Role.Application.Features.Role.CreateRole;

namespace Role.Application.Tests.Handlers.Role;

public class CreateRoleHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task CreateRole_Success()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<CreateRole>();

            var permission = _fixture.CreatePermission(request.Role.PermissionIds.First());
            context.Permissions.Add(permission);
            context.SaveChanges();

            _fixture.Inject<IRoleDbContext>(context);
            var sut = _fixture.Create<CreateRoleHandler>();

            var result = await sut.Handle(request, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }
    }
}
