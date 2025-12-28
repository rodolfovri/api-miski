namespace Miski.Domain.Entities;

public class DetalleMovimientoAlmacen
{
    public int IdDetalleMovimientoAlmacen { get; set; }
    public int IdMovimientoAlmacen { get; set; }
    public int IdVariedadProducto { get; set; }
    public int? IdLote { get; set; }
    public decimal Cantidad { get; set; }
    public int NumeroSacos { get; set; }
    // Navegation properties
    public virtual MovimientoAlmacen MovimientoAlmacen { get; set; } = null!;
    public virtual VariedadProducto VariedadProducto { get; set; } = null!;
    public virtual Lote? Lote { get; set; }
}
