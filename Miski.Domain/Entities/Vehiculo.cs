namespace Miski.Domain.Entities;

public class Vehiculo
{
    public int IdVehiculo { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public string? Estado { get; set; }

    // Navigation properties
    public virtual ICollection<CompraVehiculo> CompraVehiculos { get; set; } = new List<CompraVehiculo>();
}