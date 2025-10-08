namespace FinancasFacil.Domain.Contracts;

public interface ICalendarioRepository
{
    Task<IReadOnlyList<DateTime>> ObterFeriadosAsync(CancellationToken ct = default);
    Task<IReadOnlyList<DateTime>> ObterFeriadosEntreAsync(DateTime inicioInclusivo, DateTime fimInclusivo, CancellationToken ct = default);
}
