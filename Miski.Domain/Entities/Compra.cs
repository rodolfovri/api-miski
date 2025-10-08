namespace Miski.Domain.Entities;

public class Compra
{
    public int IdCompra { get; set; }
    public int IdNegociacion { get; set; }
    public string? Serie { get; set; }
    public DateTime? FRegistro { get; set; }
    public DateTime? FEmision { get; set; }
    public string? GuiaRemision { get; set; }
    public string? Estado { get; set; }

    // Navigation properties
    public virtual Negociacion Negociacion { get; set; } = null!;
    public virtual ICollection<Lote> Lotes { get; set; } = new List<Lote>();
    public virtual ICollection<LlegadaPlanta> LlegadasPlanta { get; set; } = new List<LlegadaPlanta>();
    public virtual ICollection<CompraVehiculo> CompraVehiculos { get; set; } = new List<CompraVehiculo>();
}