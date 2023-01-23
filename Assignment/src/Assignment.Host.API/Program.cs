using Assignment.Application;
using Assignment.Infrastructure.Extensions;
using Common.Application.Extensions;
using MediatR;
using Assignment.Infrastructure;
using Assignment.Infrastructure.Init;
using Assignment.Host.API.Filters;
using Assignment.Host.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<StatusCodeFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(IApiAssemblyMarker), typeof(IApplicationAssemblyMarker), typeof(IInfrastructureAssemblyMarker));
builder.Services.AddApiApplicationDependencies<IApplicationAssemblyMarker>();
builder.Services.AddInfrastructureDependencies(builder.Configuration);

builder.Services.Configure<AssignmentSettings>(builder.Configuration);

builder.Services.AddGrpcClient<GrpcRoleService.GrpcRoleServiceClient>(o =>
{
    o.Address = builder.Configuration.GetServiceUri("role-grpc");
});

builder.Services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

//var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
//using (var scope = scopeFactory.CreateScope())
//{
//    Stopwatch sw = new Stopwatch();
//    sw.Start();

//    var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
//    await databaseInitializer.InitRoles();

//    sw.Stop();
//    Console.WriteLine("Init took {0} ms", sw.ElapsedMilliseconds);
//}

app.Run();

public partial class Program { }