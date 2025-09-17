using Miski.Domain.Entities.Base;

namespace Miski.Domain.Entities;

public class LlegadaPlantaDetalle : BaseEntity
{
    public int IdLlegadaDetalle { get; set; }
    public int IdLlegadaPlanta { get; set; }
    public int IdLote { get; set; }
    public int SacosRecibidos { get; set; }
    public decimal PesoRecibido { get; set; }
    public int IdProductoFinal { get; set; }
    public string? Observaciones { get; set; }

    // Navigation properties
    public virtual LlegadaPlanta LlegadaPlanta { get; set; } = null!;
    public virtual Lote Lote { get; set; } = null!;
    public virtual Producto ProductoFinal { get; set; } = null!;
}