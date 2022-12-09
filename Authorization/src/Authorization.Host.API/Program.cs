using Assignment.Application;
using Authorization.Infrastructure;
using Authorization.Infrastructure.Extensions;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMediatR(typeof(IApplicationAssemblyMarker));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRepositories()
    .AddInfrastructureDependencies(builder.Configuration);

builder.Services.Configure<AuthorizationSettings>(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();