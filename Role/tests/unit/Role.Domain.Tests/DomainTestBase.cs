using AutoFixture;
using AutoFixture.AutoMoq;

namespace Role.Domain.Tests;
public class DomainTestBase
{
    protected readonly IFixture _fixture;

    public DomainTestBase()
    {
        _fixture = new Fixture()
            .Customize(new AutoMoqCustomization() { ConfigureMembers = true });

        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
}
