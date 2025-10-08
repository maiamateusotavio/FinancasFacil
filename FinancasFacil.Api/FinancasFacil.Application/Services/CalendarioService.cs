using FinancasFacil.Domain.Contracts;

namespace FinancasFacil.Application.Services;

public sealed class CalendarioService : ICalendarioService
{
    private readonly ICalendarioRepository _repo;

    public CalendarioService(ICalendarioRepository repo)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
    }

    public async Task<IReadOnlyList<DateTime>> ObterFeriadosAsync(CancellationToken ct = default)
    {
        var lista = await _repo.ObterFeriadosAsync(ct);
        return lista;
    }

    public async Task<IReadOnlyList<DateTime>> ObterFeriadosEntreAsync(DateTime inicioInclusivo, DateTime fimInclusivo, CancellationToken ct = default)
    {
        if (fimInclusivo.Date < inicioInclusivo.Date)
            throw new ArgumentException("A data final não pode ser menor que a inicial.");

        var lista = await _repo.ObterFeriadosEntreAsync(inicioInclusivo.Date, fimInclusivo.Date, ct);

        return lista
            .Distinct()
            .OrderBy(d => d)
            .ToList();
    }
}
