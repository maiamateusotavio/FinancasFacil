using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data;

namespace FinancasFacil.Api.HealthChecks;

public sealed class SqlHealthCheck : IHealthCheck
{
    private readonly IDbConnection _connection;
    public SqlHealthCheck(IDbConnection connection) => _connection = connection;

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_connection is System.Data.Common.DbConnection db && db.State != System.Data.ConnectionState.Open)
                await db.OpenAsync(cancellationToken);

            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT 1";
            var result = await (cmd as System.Data.Common.DbCommand)!.ExecuteScalarAsync(cancellationToken);
            return result is int i && i == 1
                ? HealthCheckResult.Healthy("SQL OK")
                : HealthCheckResult.Unhealthy("SQL retornou valor inesperado");
        }
        catch (System.Exception ex)
        {
            return HealthCheckResult.Unhealthy("Falha ao consultar o banco.", ex);
        }
    }
}
