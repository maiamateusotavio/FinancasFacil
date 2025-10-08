using FinancasFacil.Api.CompositionRoot;
using FinancasFacil.Api.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSwaggerWithOpenApi();

builder.Services.AddHealthChecks()
                .AddCheck<SqlHealthCheck>("database");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Finanças Fácil API v1");
});

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
