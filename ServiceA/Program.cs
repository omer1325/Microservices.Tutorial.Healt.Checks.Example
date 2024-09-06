using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddRedis(
        redisConnectionString: "localhost:1453",
        name: "Redis Check",
        failureStatus: HealthStatus.Degraded | HealthStatus.Unhealthy,
        tags: new string[] { "redis" }
        )
    .AddMongoDb(
        mongodbConnectionString: "mongodb://localhost:27017",
        name: "MongoDB Check",
        failureStatus: HealthStatus.Degraded | HealthStatus.Unhealthy,
        tags: new string[] { "mongodb" }
        )
    .AddNpgSql(
        connectionString: "User ID=postgres;Password=123456;Host=localhost;Port=5433;Database=postgres;",
        name: "PostgreSQL",
        healthQuery: "SELECT 1",
        failureStatus: HealthStatus.Degraded | HealthStatus.Unhealthy,
        tags: new string[] { "postgresql", "sql", "db" }
    );

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
