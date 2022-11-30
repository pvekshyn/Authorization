using Role.Domain;
using Role.SDK.DTO;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.EntityFrameworkCore;
using Role.Infrastructure;

namespace Role.Application.Tests;
public class ApplicationTestBase
{
    protected readonly IFixture _fixture;
    protected readonly DbContextOptions<RoleDbContext> _dbContextOptions;

    public ApplicationTestBase()
    {
        _fixture = new Fixture()
            .Customize(new AutoMoqCustomization() { ConfigureMembers = true });

        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _dbContextOptions = new DbContextOptionsBuilder<RoleDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

        _fixture.Customize<CreateRoleDto>(c => c
            .With(x => x.Name,
            () => _fixture.Create<string>().Substring(0, Constants.MaxRoleNameLength)));

        _fixture.Customize<RenameRoleDto>(c => c
            .With(x => x.Name,
            () => _fixture.Create<string>().Substring(0, Constants.MaxRoleNameLength)));
    }
}
