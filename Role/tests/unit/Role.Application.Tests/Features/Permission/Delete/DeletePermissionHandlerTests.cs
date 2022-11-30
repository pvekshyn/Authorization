using AutoFixture;
using Role.Application.Features.Permission.DeletePermission;

namespace Role.Application.Tests.Features.Permission.Delete;

public class DeletePermissionHandlerTests : ApplicationTestBase
{
    [Fact]
    public async Task DeletePermission_Success()
    {
        var request = _fixture.Create<DeletePermission>();

        _dbContext.Permissions.Add(_fixture.CreatePermission(request.PermissionId));
        _dbContext.SaveChanges();

        var sut = _fixture.Create<DeletePermissionHandler>();

        var result = await sut.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(_dbContext.Permissions);
    }
}
