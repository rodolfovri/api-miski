namespace Miski.Domain.Entities;

public class FAQ
{
    public int IdFAQ { get; set; }
    public int IdCategoriaFAQ { get; set; }
    public string Pregunta { get; set; } = string.Empty;
    public string Respuesta { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public int Orden { get; set; }
    public DateTime FRegistro { get; set; }
    // Navigation property
    public virtual CategoriaFAQ CategoriaFAQ { get; set; } = null!;
}
