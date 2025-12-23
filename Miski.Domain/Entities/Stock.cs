namespace Miski.Domain.Entities;

public class Stock
{
    public int IdStock { get; set; }
    public int IdVariedadProducto { get; set; }
    public int IdPlanta { get; set; }
    public decimal? CantidadKg { get; set; }
    public int? CantidadSacos { get; set; }
    public string TipoStock { get; set; } = null!;
    // Navigation properties
    public virtual VariedadProducto VariedadProducto { get; set; } = null!;
    public virtual Ubicacion Planta { get; set; } = null!;
}