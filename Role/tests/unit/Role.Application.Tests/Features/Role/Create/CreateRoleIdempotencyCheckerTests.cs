using Role.Infrastructure;
using AutoFixture;
using Role.Application.Dependencies;
using Role.Application.Features.Role.CreateRole;

namespace Role.Application.Tests.Features.Role.Create;
public class CreateRoleIdempotencyCheckerTests : ApplicationTestBase
{
    [Fact]
    public async Task IsOperationAlreadyApplied_True()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<CreateRole>();

            context.Roles.Add(_fixture.CreateRole(request.Role.Id, request.Role.Name));
            context.SaveChanges();

            _fixture.Inject<IRoleDbContext>(context);
            var sut = _fixture.Create<CreateRoleIdempotencyCheck>();

            var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

            Assert.True(result);
        }
    }

    [Fact]
    public async Task IsOperationAlreadyApplied_False()
    {
        using (var context = new RoleDbContext(_dbContextOptions))
        {
            var request = _fixture.Create<CreateRole>();

            _fixture.Inject<IRoleDbContext>(context);
            var sut = _fixture.Create<CreateRoleIdempotencyCheck>();

            var result = await sut.IsOperationAlreadyAppliedAsync(request, CancellationToken.None);

            Assert.False(result);
        }
    }
}
