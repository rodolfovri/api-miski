namespace Miski.Domain.Entities;

public class CategoriaPersona
{
    public int IdCategoriaPersona { get; set; }
    public string Nombre { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<PersonaCategoria> PersonaCategorias { get; set; } = new List<PersonaCategoria>();
}