using AutoFixture;
using AutoFixture.AutoMoq;

namespace Common.Application.Tests;
public class ApplicationTestBase
{
    protected readonly IFixture _fixture;

    public ApplicationTestBase()
    {
        _fixture = new Fixture()
            .Customize(new AutoMoqCustomization() { ConfigureMembers = true });

        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
}
