namespace Miski.Shared.DTOs.Maestros;

public class TipoDocumentoDto
{
    public int IdTipoDocumento { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int? LongitudMin { get; set; }
    public int? LongitudMax { get; set; }
}

public class CreateTipoDocumentoDto
{
    public string Nombre { get; set; } = string.Empty;
    public int? LongitudMin { get; set; }
    public int? LongitudMax { get; set; }
}

public class UpdateTipoDocumentoDto
{
    public string Nombre { get; set; } = string.Empty;
    public int? LongitudMin { get; set; }
    public int? LongitudMax { get; set; }
}