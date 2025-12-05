using MediatR;

namespace Miski.Application.Features.Compras.CompraPagos.Queries.GetResumenPago;

public record GetResumenPagoQuery(int IdCompra) : IRequest<ResumenPagoDto>;

public class ResumenPagoDto
{
    public int IdCompra { get; set; }
    public string? CompraSerie { get; set; }
    public string? TipoPago { get; set; }
    public decimal? MontoTotal { get; set; }
    public decimal TotalAbonado { get; set; }
    public decimal SaldoPendiente { get; set; }
    public string? EstadoPago { get; set; }
    public int? DiasCredito { get; set; }
    public DateTime? FechaUltimoPago { get; set; }
    public int TotalPagos { get; set; }
    public List<Miski.Shared.DTOs.Compras.CompraPagoDto> Pagos { get; set; } = new();
}
