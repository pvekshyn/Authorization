using Role.Domain;
using Role.SDK.DTO;
using AutoFixture;
using AutoFixture.AutoMoq;
using Role.Domain.ValueObjects.Role;

namespace Role.Application.Tests;
public class ApplicationTestBase
{
    protected readonly IFixture _fixture;

    public ApplicationTestBase()
    {
        _fixture = new Fixture()
            .Customize(new AutoMoqCustomization() { ConfigureMembers = true });

        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _fixture.Customize<CreateRoleDto>(c => c
            .With(x => x.Name,
            () => _fixture.Create<string>().Substring(0, Constants.MaxRoleNameLength)));

        _fixture.Customize<RenameRoleDto>(c => c
            .With(x => x.Name,
            () => _fixture.Create<string>().Substring(0, Constants.MaxRoleNameLength)));

        _fixture.Customize<RoleName>(c => c.FromFactory(() =>
            new RoleName(_fixture.Create<string>().Substring(0, Constants.MaxRoleNameLength))));
    }
}
