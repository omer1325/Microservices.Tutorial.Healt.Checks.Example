using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecksUI(
    settings =>
    {
        settings.AddHealthCheckEndpoint("Service A", "https://localhost:7164/health");
        settings.AddHealthCheckEndpoint("Service B", "https://localhost:7232/health");
        settings.SetEvaluationTimeInSeconds(3);
        settings.SetApiMaxActiveRequests(3);
    }).AddSqlServerStorage("Server=localhost, 1433;Database=HealthCheckUIDB;User ID=SA;Password=1q2w3e4r+!;TrustServerCertificate=True");


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
app.UseHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
