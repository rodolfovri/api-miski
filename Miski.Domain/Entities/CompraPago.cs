namespace Miski.Domain.Entities;

public class CompraPago
{
    public int IdCompraPago { get; set; }
    public int IdCompra { get; set; }
    public string TipoPago { get; set; } = string.Empty;
    public int? DiasCredito { get; set; }
    public DateTime? FPago { get; set; }
    public decimal? MontoAcuenta { get; set; }
    public decimal? Saldo { get; set; }
    public string? EstadoPago { get; set; }
    public string? Observacion { get; set; }
    public DateTime? FRegistro { get; set; }

    // Navigation properties
    public virtual Compra Compra { get; set; } = null!;
}
