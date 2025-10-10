namespace Miski.Shared.DTOs.Permisos;

public class ModuloDto
{
    public int IdModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
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
    public int Orden { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? ModuloNombre { get; set; }
    public List<SubModuloDetalleDto>? SubModuloDetalles { get; set; }
}

public class SubModuloDetalleDto
{
    public int IdSubModuloDetalle { get; set; }
    public int IdSubModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Orden { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? SubModuloNombre { get; set; }
}

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
}

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
    public int Orden { get; set; }
    public bool TieneAcceso { get; set; }
    public List<SubModuloPermisoDto> SubModulos { get; set; } = new();
}

public class SubModuloPermisoDto
{
    public int IdSubModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Orden { get; set; }
    public bool TieneAcceso { get; set; }
    public List<SubModuloDetallePermisoDto> SubModuloDetalles { get; set; } = new();
}

public class SubModuloDetallePermisoDto
{
    public int IdSubModuloDetalle { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Orden { get; set; }
    public bool TieneAcceso { get; set; }
}