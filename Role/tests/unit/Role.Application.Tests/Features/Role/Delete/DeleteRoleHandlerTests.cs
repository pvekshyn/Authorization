using Role.Infrastructure;
using AutoFixture;
using Role.Application.Dependencies;
using Role.Application.Features.Role.DeleteRole;

namespace Role.Application.Tests.Features.Role.Delete;

public class DeletePermissionHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task DeleteRole_Success()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<DeleteRole>();

            context.Roles.Add(_fixture.CreateRole(request.Id));
            context.SaveChanges();

            _fixture.Inject<IRoleDbContext>(context);
            var sut = _fixture.Create<DeleteRoleHandler>();

            var result = await sut.Handle(request, CancellationToken.None);

            Assert.True(result.IsSuccess);

            Assert.Empty(context.Roles);
        }
    }
}
