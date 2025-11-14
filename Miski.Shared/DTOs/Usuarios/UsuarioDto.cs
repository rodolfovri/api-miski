namespace Miski.Shared.DTOs.Usuarios;

public class UsuarioDto
{
    public int IdUsuario { get; set; }
    public int? IdPersona { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Estado { get; set; }
    public DateTime FRegistro { get; set; }
    
    // Información adicional de la persona
    public string? PersonaNombre { get; set; }
    public string? PersonaApellidos { get; set; }
    public string? PersonaNombreCompleto { get; set; }
    public string? PersonaEmail { get; set; }
    public string? PersonaTelefono { get; set; }
    public string? PersonaNumeroDocumento { get; set; }
    
    // Roles
    public List<string> Roles { get; set; } = new List<string>();
}

public class CreateUsuarioDto
{
    public int? IdPersona { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Estado { get; set; }
    public List<int> RolesIds { get; set; } = new List<int>();
}

public class UpdateUsuarioDto
{
    public int IdUsuario { get; set; }
    public int? IdPersona { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Estado { get; set; }
    public List<int> RolesIds { get; set; } = new List<int>();
}

public class UpdatePasswordDto
{
    public int IdUsuario { get; set; }
    public string NewPassword { get; set; } = string.Empty;
}
