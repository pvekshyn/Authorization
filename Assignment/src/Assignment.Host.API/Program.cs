using Assignment.Application;
using Assignment.Infrastructure.Extensions;
using Common.Application.Extensions;
using MediatR;
using Assignment.Infrastructure;
using Assignment.Host.API.Filters;
using Assignment.Host.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<StatusCodeFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(
    typeof(IApiAssemblyMarker),
    typeof(IApplicationAssemblyMarker),
    typeof(IInfrastructureAssemblyMarker));

builder.Services.AddApiApplicationDependencies<IApplicationAssemblyMarker>();
builder.Services.AddInfrastructureDependencies(builder.Configuration);

builder.Services.Configure<AssignmentSettings>(builder.Configuration);

builder.Services.AddGrpcClient<GrpcRoleService.GrpcRoleServiceClient>(o =>
{
    o.Address = builder.Configuration.GetServiceUri("role-grpc");
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }