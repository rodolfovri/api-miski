namespace Miski.Domain.Entities;

public class Banco
{
    public int IdBanco { get; set; }
    public string Nombre {  get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    // Navigation properties
    public virtual ICollection<Negociacion> Negociaciones { get; set; } = new List<Negociacion>();
}
