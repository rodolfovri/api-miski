namespace Miski.Shared.DTOs.Personas;

public class PersonaCategoriaDto
{
    public int IdPersonaCategoria { get; set; }
    public int IdPersona { get; set; }
    public int IdCategoria { get; set; }
    
    // Datos adicionales de la categoría
    public string? CategoriaNombre { get; set; }
}

public class AsignarCategoriaDto
{
    public int IdPersona { get; set; }
    public int IdCategoria { get; set; }
}

public class RevocarCategoriaDto
{
    public int IdPersona { get; set; }
    public int IdCategoria { get; set; }
}
