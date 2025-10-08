using FinancasFacil.Application.Services;
using FinancasFacil.Domain.Contracts;
using FinancasFacil.Infrastructure.Persistence.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace FinancasFacil.Api.CompositionRoot;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICalendarioService, CalendarioService>();
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IDbConnection>(_ =>
            new SqlConnection(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICalendarioRepository, CalendarioRepository>();
        return services;
    }
}
