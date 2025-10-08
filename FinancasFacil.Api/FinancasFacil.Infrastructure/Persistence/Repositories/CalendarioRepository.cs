using Dapper;
using FinancasFacil.Domain.Contracts;
using System.Data;

namespace FinancasFacil.Infrastructure.Persistence.Repositories;

public sealed class CalendarioRepository : ICalendarioRepository
{
    private readonly IDbConnection _connection;

    public CalendarioRepository(IDbConnection connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public async Task<IReadOnlyList<DateTime>> ObterFeriadosAsync(CancellationToken ct = default)
    {
        const string sql = @"
                SELECT [Data]
                FROM [Feriados]
                ORDER BY [Data];";

        await EnsureOpenAsync(ct);
        var datas = await _connection.QueryAsync<DateTime>(new CommandDefinition(sql, cancellationToken: ct));
        // Observação: SQL DATE vira DateTime com Kind = Unspecified em .NET.
        return datas.AsList();
    }

    public async Task<IReadOnlyList<DateTime>> ObterFeriadosEntreAsync(DateTime inicioInclusivo, DateTime fimInclusivo, CancellationToken ct = default)
    {
        const string sql = @"
                SELECT [Data]
                FROM [Feriados]
                WHERE [Data] >= @inicio AND [Data] <= @fim
                ORDER BY [Data];";

        var param = new { inicio = inicioInclusivo.Date, fim = fimInclusivo.Date };

        await EnsureOpenAsync(ct);
        var datas = await _connection.QueryAsync<DateTime>(
            new CommandDefinition(sql, parameters: param, cancellationToken: ct));

        return datas.AsList();
    }

    private async Task EnsureOpenAsync(CancellationToken ct)
    {
        if (_connection.State != ConnectionState.Open && _connection is System.Data.Common.DbConnection db)
        {
            await db.OpenAsync(ct);
        }
    }
}
