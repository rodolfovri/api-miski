namespace Miski.Shared.DTOs.Permisos;

public class AsignarPermisoDto
{
    public int IdRol { get; set; }
    public int? IdModulo { get; set; }
    public int? IdSubModulo { get; set; }
    public int? IdSubModuloDetalle { get; set; }
    public bool TieneAcceso { get; set; } = true;
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
}

public class CreateModuloDto
{
    public string Nombre { get; set; } = string.Empty;
    public int Orden { get; set; }
    public string Estado { get; set; } = "ACTIVO";
    public string TipoPlataforma { get; set; } = "Web";
}

public class UpdateModuloDto
{
    public int IdModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Orden { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string TipoPlataforma { get; set; } = string.Empty;
}

public class CreateSubModuloDto
{
    public int IdModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Orden { get; set; }
    public string Estado { get; set; } = "ACTIVO";
}

public class UpdateSubModuloDto
{
    public int IdSubModulo { get; set; }
    public int IdModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Orden { get; set; }
    public string Estado { get; set; } = string.Empty;
}

public class CreateSubModuloDetalleDto
{
    public int IdSubModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Orden { get; set; }
    public string Estado { get; set; } = "ACTIVO";
}

public class UpdateSubModuloDetalleDto
{
    public int IdSubModuloDetalle { get; set; }
    public int IdSubModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Orden { get; set; }
    public string Estado { get; set; } = string.Empty;
}