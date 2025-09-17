namespace Miski.Shared.DTOs;

public class PersonaDto
{
    public int IdPersona { get; set; }
    public int IdTipoDocumento { get; set; }
    public string TipoDocumentoNombre { get; set; } = string.Empty;
    public string NumeroDocumento { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
    public string? Estado { get; set; }
    public DateTime FRegistro { get; set; }
}