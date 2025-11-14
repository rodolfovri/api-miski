namespace Miski.Domain.Entities;

public class Compra
{
    public int IdCompra { get; set; }
    public int IdNegociacion { get; set; }
    public int? IdLote { get; set; }
    public int IdMoneda { get; set; }
    public int? IdTipoCambio { get; set; }
    public string? Serie { get; set; }
    public string? Correlativo { get; set; }
    public DateTime? FRegistro { get; set; }
    public DateTime? FEmision { get; set; }
    public decimal? MontoTotal { get; set; }
    public decimal? IGV { get; set; }
    public string? Observacion { get; set; }
    public string? Estado { get; set; }
    public string? EstadoRecepcion { get; set; }
    public string? EsParcial { get; set; }
    public int? IdUsuarioAnulacion { get; set; }
    public string? MotivoAnulacion { get; set; }
    public DateTime? FAnulacion { get; set; } 

    // Navigation properties
    public virtual Negociacion Negociacion { get; set; } = null!;
    public virtual Moneda Moneda { get; set; } = null!;
    public virtual TipoCambio? TipoCambio { get; set; }
    public virtual Usuario? UsuarioAnulacion { get; set; }
    public virtual Lote? Lote { get; set; }
    public virtual ICollection<LlegadaPlanta> LlegadasPlanta { get; set; } = new List<LlegadaPlanta>();
    public virtual ICollection<CompraVehiculoDetalle> CompraVehiculoDetalles { get; set; } = new List<CompraVehiculoDetalle>();

}