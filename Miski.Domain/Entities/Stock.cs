namespace Miski.Domain.Entities;

public class Stock
{
    public int IdStock { get; set; }
    public int IdVariedadProducto { get; set; }
    public int IdPlanta { get; set; }
    public decimal? CantidadKg { get; set; }
    // Navigation properties
    public virtual VariedadProducto VariedadProducto { get; set; } = null!;
    public virtual Ubicacion Planta { get; set; } = null!;
}