namespace Miski.Domain.Entities;

public class Stock
{
    public int IdStock { get; set; }
    public int IdProducto { get; set; }
    public int IdPlanta { get; set; }
    public decimal? CantidadKg { get; set; }
    public int? CantidadSacos { get; set; }

    // Navigation properties
    public virtual Producto Producto { get; set; } = null!;
    public virtual Ubicacion Planta { get; set; } = null!;
}