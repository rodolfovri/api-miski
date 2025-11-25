namespace Miski.Shared.DTOs.Permisos;

// ==================== DTOs de Acción ====================
public class AccionDto
{
    public int IdAccion { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Icono { get; set; } = string.Empty;
    public int Orden { get; set; }
    public string Estado { get; set; } = string.Empty;
}

public class AccionPermisoDto
{
    public int IdAccion { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Icono { get; set; } = string.Empty;
    public bool Habilitado { get; set; } // Si el rol tiene este permiso de acción
}

// ==================== DTOs de Módulos (actualizados) ====================
public class ModuloDto
{
    public int IdModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Ruta { get; set; }
    public string? Icono { get; set; }
    public int Orden { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string TipoPlataforma { get; set; } = string.Empty;
    public List<SubModuloDto>? SubModulos { get; set; }
}

public class SubModuloDto
{
    public int IdSubModulo { get; set; }
    public int IdModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Ruta { get; set; }
    public string? Icono { get; set; }
    public int Orden { get; set; }
    public string Estado { get; set; } = string.Empty;
    public bool TieneDetalles { get; set; } // true = tiene SubModuloDetalles, false = tiene acciones directas
    public string? ModuloNombre { get; set; }
    public List<SubModuloDetalleDto>? SubModuloDetalles { get; set; }
    public List<AccionDto>? Acciones { get; set; } // Acciones disponibles si TieneDetalles = false
}

public class SubModuloDetalleDto
{
    public int IdSubModuloDetalle { get; set; }
    public int IdSubModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Ruta { get; set; }
    public string? Icono { get; set; }
    public int Orden { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? SubModuloNombre { get; set; }
    public List<AccionDto>? Acciones { get; set; } // Acciones disponibles para este detalle
}

// ==================== DTOs de Permisos ====================
public class PermisoRolDto
{
    public int IdPermisoRol { get; set; }
    public int IdRol { get; set; }
    public int? IdModulo { get; set; }
    public int? IdSubModulo { get; set; }
    public int? IdSubModuloDetalle { get; set; }
    public bool TieneAcceso { get; set; }
    
    // Información adicional para la UI
    public string? RolNombre { get; set; }
    public string? ModuloNombre { get; set; }
    public string? SubModuloNombre { get; set; }
    public string? SubModuloDetalleNombre { get; set; }
    
    // Acciones asociadas a este permiso
    public List<AccionPermisoDto>? Acciones { get; set; }
}

// ==================== DTOs con Jerarquía para Permisos de Rol ====================
public class PermisoRolConJerarquiaDto
{
    public int IdRol { get; set; }
    public string RolNombre { get; set; } = string.Empty;
    public List<ModuloPermisoDto> Modulos { get; set; } = new();
}

public class ModuloPermisoDto
{
    public int IdModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Ruta { get; set; }
    public string? Icono { get; set; }
    public int Orden { get; set; }
    public bool TieneAcceso { get; set; }
    public List<SubModuloPermisoDto> SubModulos { get; set; } = new();
}

public class SubModuloPermisoDto
{
    public int IdSubModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Ruta { get; set; }
    public string? Icono { get; set; }
    public int Orden { get; set; }
    public bool TieneAcceso { get; set; }
    public bool TieneDetalles { get; set; }
    public List<SubModuloDetallePermisoDto>? SubModuloDetalles { get; set; }
    public List<AccionPermisoDto>? Acciones { get; set; } // Si TieneDetalles = false
}

public class SubModuloDetallePermisoDto
{
    public int IdSubModuloDetalle { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Ruta { get; set; }
    public string? Icono { get; set; }
    public int Orden { get; set; }
    public bool TieneAcceso { get; set; }
    public List<AccionPermisoDto>? Acciones { get; set; }
}