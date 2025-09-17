namespace Miski.Domain.Entities;

public class LlegadaPlanta
{
    public int IdLlegadaPlanta { get; set; }
    public int IdCompra { get; set; }
    public int IdUsuario { get; set; }
    public DateTime? FLlegada { get; set; }
    public string? Observaciones { get; set; }
    public string? Estado { get; set; }

    // Navigation properties
    public virtual Compra Compra { get; set; } = null!;
    public virtual Persona Usuario { get; set; } = null!;
    public virtual ICollection<LlegadaPlantaDetalle> LlegadaPlantaDetalles { get; set; } = new List<LlegadaPlantaDetalle>();
}