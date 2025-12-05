using MediatR;

namespace Miski.Application.Features.Compras.CompraPagos.Queries.GetComprasConEstadoPago;

public record GetComprasConEstadoPagoQuery(
    string? EstadoPago = null,
    string? TipoPago = null
) : IRequest<List<CompraConEstadoPagoDto>>;

public class CompraConEstadoPagoDto
{
    public int IdCompra { get; set; }
    public string? Serie { get; set; }
    public decimal? MontoTotal { get; set; }
    public string? TipoPago { get; set; }
    public string EstadoPago { get; set; } = string.Empty;
    public decimal TotalAbonado { get; set; }
    public decimal SaldoPendiente { get; set; }
    public int? DiasCredito { get; set; }
    public DateTime? FechaUltimoPago { get; set; }
    public DateTime? FRegistro { get; set; }
    public int TotalPagos { get; set; }
}
