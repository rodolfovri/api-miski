namespace Miski.Shared.DTOs.Maestros;

public class ConfiguracionGlobalDto
{
    public int IdConfiguracionGlobal { get; set; }
    public string Clave { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string TipoDato { get; set; } = string.Empty;
    public bool EsEditable { get; set; }
    public DateTime FRegistro { get; set; }
    public DateTime? FModificacion { get; set; }
}

public class CreateConfiguracionGlobalDto
{
    public string Clave { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string TipoDato { get; set; } = string.Empty;
    public bool EsEditable { get; set; } = true;
}

public class UpdateConfiguracionGlobalDto
{
    public int IdConfiguracionGlobal { get; set; }
    public string Valor { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
}
