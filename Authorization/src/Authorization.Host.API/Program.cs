using Authorization.Infrastructure;
using Authorization.Infrastructure.DataAccess.Read;
using Authorization.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAccessRepository();

builder.Services.Configure<AuthorizationSettings>(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/checkaccess/userId/{userId}/permissionId/{permissionId}",
    (Guid userId, Guid permissionId, IAccessRepository _repository) =>
    _repository.CheckAccess(userId, permissionId));

app.Run();