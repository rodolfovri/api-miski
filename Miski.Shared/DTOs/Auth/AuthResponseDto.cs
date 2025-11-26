namespace Miski.Shared.DTOs.Auth;

public class AuthResponseDto
{
    public int IdUsuario { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public AuthPersonaDto? Persona { get; set; }
    public List<RolDto> Roles { get; set; } = new List<RolDto>();
}

public class RolDto : IEquatable<RolDto>
{
    public int IdRol { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? TipoPlataforma { get; set; }
    public List<RolPermisoDto> Permisos { get; set; } = new List<RolPermisoDto>();

    // Implementación de IEquatable para Distinct()
    public bool Equals(RolDto? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return IdRol == other.IdRol;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as RolDto);
    }

    public override int GetHashCode()
    {
        return IdRol.GetHashCode();
    }
}

public class RolPermisoDto
{
    public int? IdModulo { get; set; }
    public string? ModuloNombre { get; set; }
    public string? ModuloRuta { get; set; }
    public string? ModuloIcono { get; set; }
    public int? IdSubModulo { get; set; }
    public string? SubModuloNombre { get; set; }
    public string? SubModuloRuta { get; set; }
    public string? SubModuloIcono { get; set; }
    public bool? SubModuloTieneDetalles { get; set; }
    public int? IdSubModuloDetalle { get; set; }
    public string? SubModuloDetalleNombre { get; set; }
    public string? SubModuloDetalleRuta { get; set; }
    public string? SubModuloDetalleIcono { get; set; }
    public bool TieneAcceso { get; set; }
    
    /// <summary>
    /// Acciones disponibles para esta pantalla con su estado de habilitación
    /// </summary>
    public List<RolAccionDto> Acciones { get; set; } = new List<RolAccionDto>();
}

/// <summary>
/// Representa una acción disponible con su estado de habilitación para el rol
/// </summary>
public class RolAccionDto
{
    public int IdAccion { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Icono { get; set; } = string.Empty;
    public int Orden { get; set; }
    
    /// <summary>
    /// Indica si el rol tiene habilitada esta acción
    /// </summary>
    public bool Habilitado { get; set; }
}

public class AuthPersonaDto
{
    public int IdPersona { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string NumeroDocumento { get; set; } = string.Empty;
    public string TipoDocumentoNombre { get; set; } = string.Empty;
}