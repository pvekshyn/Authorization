using Assignment.Application;
using Authorization.Infrastructure;
using Authorization.Infrastructure.Extensions;
using Authorization.Infrastructure.Grpc;
using Common.Application.Extensions;
using Inbox.SDK.Extensions;
using Inbox.SDK.Grpc;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMediatR(typeof(IApplicationAssemblyMarker), typeof(IInfrastructureAssemblyMarker));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiApplicationDependencies<IApplicationAssemblyMarker>()
    .AddInfrastructureDependencies(builder.Configuration)
    .AddEventToRequestMappers<IInfrastructureAssemblyMarker>();

builder.Services.Configure<AuthorizationSettings>(builder.Configuration);

builder.Services.AddGrpc();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.MapGrpcService<EventProcessingService>();
app.MapGrpcService<CheckAccessService>();

app.Run();