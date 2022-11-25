using Common.Application.Extensions;
using MediatR;
using Role.API;
using Role.API.Filters;
using Role.Application;
using Role.Infrastructure;
using Role.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<StatusCodeFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(IApiAssemblyMarker), typeof(IApplicationAssemblyMarker), typeof(IInfrastructureAssemblyMarker));
builder.Services.AddApplicationDependencies<IApplicationAssemblyMarker>();
builder.Services.AddInfrastructureDependencies(builder.Configuration);

builder.Services.Configure<RoleSettings>(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();