namespace Miski.Shared.DTOs.Permisos;

// ==================== DTOs para Asignar Permisos ====================
public class AsignarPermisoDto
{
    public int IdRol { get; set; }
    public int? IdModulo { get; set; }
    public int? IdSubModulo { get; set; }
    public int? IdSubModuloDetalle { get; set; }
    public bool TieneAcceso { get; set; } = true;
    public List<int>? IdAcciones { get; set; } // IDs de acciones a habilitar
}

public class AsignarPermisosMultiplesDto
{
    public int IdRol { get; set; }
    public List<PermisoItemDto> Permisos { get; set; } = new();
}

public class PermisoItemDto
{
    public int? IdModulo { get; set; }
    public int? IdSubModulo { get; set; }
    public int? IdSubModuloDetalle { get; set; }
    public bool TieneAcceso { get; set; } = true;
    public List<int>? IdAcciones { get; set; } // IDs de acciones a habilitar
}

// ==================== DTOs para Acciones ====================
public class CreateAccionDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Icono { get; set; } = string.Empty;
    public int Orden { get; set; }
    public string Estado { get; set; } = "ACTIVO";
}

public class UpdateAccionDto
{
    public int IdAccion { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Icono { get; set; } = string.Empty;
    public int Orden { get; set; }
    public string Estado { get; set; } = string.Empty;
}

// ==================== DTOs para Módulos ====================
public class CreateModuloDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Ruta { get; set; }
    public string? Icono { get; set; }
    public int Orden { get; set; }
    public string Estado { get; set; } = "ACTIVO";
    public string TipoPlataforma { get; set; } = "Web";
}

public class UpdateModuloDto
{
    public int IdModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Ruta { get; set; }
    public string? Icono { get; set; }
    public int Orden { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string TipoPlataforma { get; set; } = string.Empty;
}

// ==================== DTOs para SubMódulos ====================
public class CreateSubModuloDto
{
    public int IdModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Ruta { get; set; }
    public string? Icono { get; set; }
    public int Orden { get; set; }
    public bool TieneDetalles { get; set; } // true = tiene SubModuloDetalles, false = acciones directas
    public string Estado { get; set; } = "ACTIVO";
}

public class UpdateSubModuloDto
{
    public int IdSubModulo { get; set; }
    public int IdModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Ruta { get; set; }
    public string? Icono { get; set; }
    public int Orden { get; set; }
    public bool TieneDetalles { get; set; }
    public string Estado { get; set; } = string.Empty;
}

// ==================== DTOs para SubMóduloDetalles ====================
public class CreateSubModuloDetalleDto
{
    public int IdSubModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Ruta { get; set; }
    public string? Icono { get; set; }
    public int Orden { get; set; }
    public string Estado { get; set; } = "ACTIVO";
}

public class UpdateSubModuloDetalleDto
{
    public int IdSubModuloDetalle { get; set; }
    public int IdSubModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Ruta { get; set; }
    public string? Icono { get; set; }
    public int Orden { get; set; }
    public string Estado { get; set; } = string.Empty;
}