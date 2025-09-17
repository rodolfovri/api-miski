namespace Miski.Domain.Entities;

public class PersonaCategoria
{
    public int IdPersonaCategoria { get; set; }
    public int IdPersona { get; set; }
    public int IdCategoria { get; set; }

    // Navigation properties
    public virtual Persona Persona { get; set; } = null!;
    public virtual CategoriaPersona Categoria { get; set; } = null!;
}