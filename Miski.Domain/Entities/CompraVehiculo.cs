namespace Miski.Domain.Entities;

public class CompraVehiculo
{
    public int IdCompraVehiculo { get; set; }
    public int IdPersona { get; set; }
    public int IdVehiculo { get; set; }
    public string GuiaRemision { get; set; } = string.Empty;
    public string? Estado { get; set; }
    public DateTime FRegistro { get; set; }

    // Navigation properties
    public virtual Vehiculo Vehiculo { get; set; } = null!;
    public virtual Persona Persona { get; set; } = null!;
    public virtual ICollection<CompraVehiculoDetalle> CompraVehiculoDetalles { get; set; } = new List<CompraVehiculoDetalle>();

}
