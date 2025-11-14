namespace Miski.Domain.Entities;

public class Lote
{
    public int IdLote { get; set; }
    public decimal Peso { get; set; }
    public int Sacos { get; set; }
    public string? Codigo { get; set; }
    public decimal? Comision { get; set; }
    public string? Observacion { get; set; }
    
    // Navigation properties
    public virtual ICollection<LlegadaPlanta> LlegadasPlanta { get; set; } = new List<LlegadaPlanta>();
    
    // ? RELACIÓN UNO A UNO: Un Lote puede estar asociado a una Compra
    public virtual Compra? Compra { get; set; }
}