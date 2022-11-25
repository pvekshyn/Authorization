using Assignment.Infrastructure.Extensions;
using Assignment.Application;
using Assignment.Infrastructure;
using MediatR;
using Common.Application.Extensions;
using Assignment.Infrastructure.Grpc;
using Inbox.SDK.Extensions;
using Inbox.SDK.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AssignmentSettings>(builder.Configuration);

builder.Services.AddGrpcApplicationDependencies<IApplicationAssemblyMarker>()
    .AddInfrastructureDependencies(builder.Configuration)
    .AddEventToRequestMappers<IInfrastructureAssemblyMarker>();

builder.Services.AddMediatR(typeof(IApplicationAssemblyMarker), typeof(IInfrastructureAssemblyMarker));

builder.Services.AddGrpc();

var app = builder.Build();


app.MapGrpcService<AssignmentService>();
app.MapGrpcService<EventProcessingService>();

app.Run();

public partial class Program { }
