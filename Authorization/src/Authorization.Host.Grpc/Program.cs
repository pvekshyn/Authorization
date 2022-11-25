using Assignment.Application;
using Authorization.Infrastructure;
using Authorization.Infrastructure.Extensions;
using Common.Application.Extensions;
using Inbox.SDK.Extensions;
using Inbox.SDK.Grpc;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AuthorizationSettings>(builder.Configuration);

builder.Services.AddGrpcApplicationDependencies<IApplicationAssemblyMarker>()
    .AddRepositories()
    .AddEventToRequestMappers<IInfrastructureAssemblyMarker>();

builder.Services.AddMediatR(typeof(IApplicationAssemblyMarker), typeof(IInfrastructureAssemblyMarker));

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<EventProcessingService>();

app.Run();