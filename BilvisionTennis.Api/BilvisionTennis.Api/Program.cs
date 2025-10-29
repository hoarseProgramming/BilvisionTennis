using BilvisionTennisAPI.GraphQL.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddDbContext<TennisDbContext>(
        options => options.UseNpgsql("Host=127.0.0.1;Username=tennis;Password=secret"))
    .AddGraphQLServer()
    .AddMutationConventions()
    .AddApiTypes();

var app = builder.Build();

app.MapGraphQL();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TennisDbContext>();

    db.Database.Migrate();

}


await app.RunWithGraphQLCommandsAsync(args);
