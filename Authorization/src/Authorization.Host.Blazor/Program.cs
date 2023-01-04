using Authorization.Infrastructure;
using Authorization.Infrastructure.Extensions;
using Authorization.Infrastructure.Init;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddInfrastructureDependencies(builder.Configuration)
    .AddTransient<IDatabaseInitializer, DatabaseInitializer>();

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.Configure<AuthorizationSettings>(builder.Configuration);

builder.Services.AddGrpcClient<GrpcRoleService.GrpcRoleServiceClient>(o =>
{
    o.Address = GetGrpcUri(builder.Configuration, "role-grpc");
});

builder.Services.AddGrpcClient<GrpcAssignmentService.GrpcAssignmentServiceClient>(o =>
{
    o.Address = GetGrpcUri(builder.Configuration, "assignment-grpc");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

Uri? GetGrpcUri(IConfiguration configuration, string serviceName)
{
    var grpcServiceUri = configuration.GetServiceUri(serviceName);
    if (grpcServiceUri is null)
        throw new Exception("Cannot get Service Uri");

    return grpcServiceUri;
}
