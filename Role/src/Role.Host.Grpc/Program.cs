using Role.Infrastructure;
using Role.Infrastructure.Extensions;
using Role.Infrastructure.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RoleSettings>(builder.Configuration);
builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<RoleService>();

app.Run();
