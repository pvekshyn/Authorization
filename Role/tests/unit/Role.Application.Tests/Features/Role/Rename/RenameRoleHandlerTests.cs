using Role.Infrastructure;
using AutoFixture;
using Role.Application.Dependencies;
using Role.Application.Features.Role.RenameRole;

namespace Role.Application.Tests.Features.Role.Rename;

public class RenameRoleHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task RenameRole_Success()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<RenameRole>();

            context.Roles.Add(_fixture.CreateRole(request.Role.Id));
            context.SaveChanges();

            _fixture.Inject<IRoleDbContext>(context);
            var sut = _fixture.Create<RenameRoleHandler>();

            var result = await sut.Handle(request, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }
    }
}
