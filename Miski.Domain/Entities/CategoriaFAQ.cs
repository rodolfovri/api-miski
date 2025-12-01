namespace Miski.Domain.Entities;

public class CategoriaFAQ
{
    public int IdCategoriaFAQ { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    // Navigation property
    public virtual ICollection<FAQ> FAQs { get; set; } = new List<FAQ>();
}
