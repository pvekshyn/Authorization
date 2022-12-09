using Authorization.Infrastructure.Extensions;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureDependencies(builder.Configuration);

builder.Services.AddControllers()
        .AddOData(options =>
            {
                options.EnableQueryFeatures().AddRouteComponents("odata", GetEdmModel());
            });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();

static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EnableLowerCamelCase();
    builder.EntitySet<Authorization.Domain.Role>("Roles");
    return builder.GetEdmModel();
}
