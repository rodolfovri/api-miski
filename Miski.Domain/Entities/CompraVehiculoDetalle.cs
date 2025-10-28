namespace Miski.Domain.Entities;

public class CompraVehiculoDetalle
{
    public int IdCompraVehiculoDetalle { get; set; }
    public int IdCompraVehiculo { get; set; }
    public int IdCompra { get; set; }

    // Navigation properties
    public virtual CompraVehiculo CompraVehiculo { get; set; } = null!;
    public virtual Compra Compra { get; set; } = null!;
}
