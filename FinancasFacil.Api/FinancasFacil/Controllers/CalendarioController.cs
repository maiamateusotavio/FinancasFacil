using FinancasFacil.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace FinancasFacil.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CalendarioController : ControllerBase
{
    private readonly ICalendarioService _service;

    public CalendarioController(ICalendarioService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retorna feriados. Informe opcionalmente um intervalo (inicio, fim).
    /// Formatos aceitos: yyyy-MM-dd ou dd/MM/yyyy.
    /// </summary>
    [HttpGet("feriados")]
    [ProducesResponseType(typeof(IReadOnlyList<DateTime>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetFeriados(
        [FromQuery] string? inicio,
        [FromQuery] string? fim,
        CancellationToken ct)
    {
        if (!TryParseDate(inicio, out var inicioDt)) inicioDt = null;
        if (!TryParseDate(fim, out var fimDt)) fimDt = null;

        if (inicioDt.HasValue && fimDt.HasValue && fimDt.Value.Date < inicioDt.Value.Date)
            return BadRequest("A data final não pode ser menor que a data inicial.");

        IReadOnlyList<DateTime> datas;
        if (inicioDt.HasValue || fimDt.HasValue)
        {
            var i = inicioDt ?? DateTime.MinValue.Date;
            var f = fimDt ?? DateTime.MaxValue.Date;
            datas = await _service.ObterFeriadosEntreAsync(i, f, ct);
        }
        else
        {
            datas = await _service.ObterFeriadosAsync(ct);
        }

        return Ok(datas);
    }

    private static bool TryParseDate(string? value, out DateTime? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(value)) return false;

        var formats = new[] { "yyyy-MM-dd", "dd/MM/yyyy" };
        if (DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeLocal, out var dt))
        {
            result = dt.Date;
            return true;
        }
        return false;
    }
}
