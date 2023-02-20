using Authorization.Infrastructure;
using Authorization.Infrastructure.Extensions;
using Authorization.Infrastructure.Init;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var keyVaultName = builder.Configuration.GetSection("KeyVaultName")?.Value;
if (!string.IsNullOrEmpty(keyVaultName))
{
    var managedIdentityClientId = builder.Configuration.GetSection("ManagedIdentityClientId")?.Value;
    var options = new DefaultAzureCredentialOptions { ManagedIdentityClientId = managedIdentityClientId };
    var keyVaultEndpoint = $"https://{keyVaultName}.vault.azure.net";
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultEndpoint),
        new DefaultAzureCredential(options));
}

builder.Services.AddInfrastructureDependencies(builder.Configuration)
    .AddTransient<IDatabaseInitializer, DatabaseInitializer>();

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.Configure<AuthorizationSettings>(builder.Configuration);

builder.Services.AddGrpcClient<GrpcRoleService.GrpcRoleServiceClient>(o =>
{
    o.Address = GetGrpcUri(builder.Configuration, "role-api");
});

builder.Services.AddGrpcClient<GrpcAssignmentService.GrpcAssignmentServiceClient>(o =>
{
    o.Address = GetGrpcUri(builder.Configuration, "assignment-api");
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
    var grpcServiceUri = configuration.GetServiceUri(serviceName, "grpc");
    if (grpcServiceUri is null)
        throw new Exception($"Cannot get {serviceName} Uri");

    return grpcServiceUri;
}
