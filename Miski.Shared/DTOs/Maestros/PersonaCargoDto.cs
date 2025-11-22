namespace Miski.Shared.DTOs.Maestros;

public class PersonaCargoDto
{
    public int IdPersonaCargo { get; set; }
    public int IdPersona { get; set; }
    public int IdCargo { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public bool EsActual { get; set; }
    public string? ObservacionAsignacion { get; set; }
    public string? MotivoRevocacion { get; set; }
    
    // Datos adicionales
    public string? PersonaNombre { get; set; }
    public string? CargoNombre { get; set; }
}

public class AsignarCargoDto
{
    public int IdPersona { get; set; }
    public int IdCargo { get; set; }
    public DateTime FechaInicio { get; set; }
    public string? ObservacionAsignacion { get; set; }
}

public class RevocarCargoDto
{
    public int IdPersonaCargo { get; set; }
    public DateTime FechaFin { get; set; }
    public string? MotivoRevocacion { get; set; }
}

public class GetCargosByPersonaDto
{
    public int IdPersona { get; set; }
    public bool? SoloActuales { get; set; }
}
