using Authorization.GraphQL;
using Authorization.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureDependencies(builder.Configuration);

builder.Services.AddGraphQLServer().AddQueryType<Query>().AddProjections().AddFiltering().AddSorting();

var app = builder.Build();

app.MapGraphQL("/graphql");

app.Run();
