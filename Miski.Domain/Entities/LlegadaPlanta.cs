namespace Miski.Domain.Entities;

public class LlegadaPlanta
{
    public int IdLlegadaPlanta { get; set; }
    public int IdCompra { get; set; }
    public int IdUsuario { get; set; }
    public int IdLote { get; set; }
    public int IdUbicacion { get; set; }
    public double SacosRecibidos { get; set; }
    public double PesoRecibido { get; set; }
    public DateTime? FLlegada { get; set; }
    public string? Observaciones { get; set; }
    public string? Estado { get; set; }

    // Navigation properties
    public virtual Compra Compra { get; set; } = null!;
    public virtual Usuario Usuario { get; set; } = null!;
    public virtual Lote Lote { get; set; } = null!;
    public virtual Ubicacion Ubicacion { get; set; } = null!;
}