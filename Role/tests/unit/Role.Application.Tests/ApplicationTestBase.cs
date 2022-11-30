using Role.Domain;
using Role.SDK.DTO;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.EntityFrameworkCore;
using Role.Infrastructure;
using Role.Application.Dependencies;

namespace Role.Application.Tests;
public class ApplicationTestBase : IDisposable
{
    protected readonly IFixture _fixture;
    protected readonly RoleDbContext _dbContext;

    public ApplicationTestBase()
    {
        _fixture = new Fixture()
            .Customize(new AutoMoqCustomization() { ConfigureMembers = true });

        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var dbContextOptions = new DbContextOptionsBuilder<RoleDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

        _dbContext = new RoleDbContext(dbContextOptions);
        _fixture.Inject<IRoleDbContext>(_dbContext);

        _fixture.Customize<CreateRoleDto>(c => c
            .With(x => x.Name,
            () => _fixture.Create<string>().Substring(0, Constants.MaxRoleNameLength)));

        _fixture.Customize<RenameRoleDto>(c => c
            .With(x => x.Name,
            () => _fixture.Create<string>().Substring(0, Constants.MaxRoleNameLength)));
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
