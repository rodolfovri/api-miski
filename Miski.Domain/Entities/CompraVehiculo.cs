namespace Miski.Domain.Entities;

public class CompraVehiculo
{
    public int IdCompraVehiculo { get; set; }
    public int IdVehiculo { get; set; }
    public string GuiaRemision { get; set; } = string.Empty;
    public DateTime FRegistro { get; set; }

    // Navigation properties
    public virtual Vehiculo Vehiculo { get; set; } = null!;
    public virtual ICollection<CompraVehiculoDetalle> CompraVehiculoDetalles { get; set; } = new List<CompraVehiculoDetalle>();

}
