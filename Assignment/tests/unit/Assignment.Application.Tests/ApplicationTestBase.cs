using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.EntityFrameworkCore;
using Assignment.Infrastructure;

namespace Assignment.Application.Tests;
public class ApplicationTestBase
{
    protected readonly IFixture _fixture;
    protected readonly DbContextOptions<AssignmentDbContext> _dbContextOptions;

    public ApplicationTestBase()
    {
        _fixture = new Fixture()
            .Customize(new AutoMoqCustomization() { ConfigureMembers = true });

        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _dbContextOptions = new DbContextOptionsBuilder<AssignmentDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;
    }
}
