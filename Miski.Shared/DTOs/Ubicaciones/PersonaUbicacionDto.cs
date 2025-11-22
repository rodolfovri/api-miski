namespace Miski.Shared.DTOs.Ubicaciones;

public class PersonaUbicacionDto
{
    public int IdPersonaUbicacion { get; set; }
    public int IdPersona { get; set; }
    public int IdUbicacion { get; set; }
    public DateTime FRegistro { get; set; }
    
    // Datos adicionales de la ubicación
    public string? UbicacionNombre { get; set; }
    public string? UbicacionTipo { get; set; }
    public string? UbicacionDireccion { get; set; }
    public string? UbicacionEstado { get; set; }
}

public class AsignarUbicacionDto
{
    public int IdPersona { get; set; }
    public int IdUbicacion { get; set; }
}

public class RevocarUbicacionDto
{
    public int IdPersona { get; set; }
    public int IdUbicacion { get; set; }
}
