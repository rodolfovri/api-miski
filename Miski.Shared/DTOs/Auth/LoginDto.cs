namespace Miski.Shared.DTOs.Auth;

public class LoginDto
{
    public string NumeroDocumento { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string TipoPlataforma { get; set; } = "Web"; // Web, Mobile, etc.

    // Campos adicionales para Mobile (opcionales para Web)
    public string? DeviceId { get; set; }
    public string? ModeloDispositivo { get; set; }
    public string? SistemaOperativo { get; set; }
    public string? VersionApp { get; set; }
}

