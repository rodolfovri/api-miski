namespace Miski.Domain.Entities;

public class ConfiguracionGlobal
{
    public int IdConfiguracionGlobal { get; set; }
    public string Clave { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string TipoDato { get; set; } = string.Empty; // "decimal", "int", "string", "bool"
    public bool EsEditable { get; set; }
    public DateTime FRegistro { get; set; }
    public DateTime? FModificacion { get; set; }
}
