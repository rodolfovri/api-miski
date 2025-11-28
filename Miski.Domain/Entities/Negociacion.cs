namespace Miski.Domain.Entities;

public class Negociacion
{
    public int IdNegociacion { get; set; }
    public int IdComisionista { get; set; }
    public int? IdPersonaProveedor { get; set; }
    public int? IdTipoDocumento { get; set; }
    public int? IdBanco { get; set; }
    public string NroDocumentoProveedor { get; set; } = string.Empty;
    public int? IdVariedadProducto { get; set; }
    public string TipoCalidad { get; set; } = string.Empty;
    public decimal? PrecioUnitario { get; set; }
    public int? SacosTotales { get; set; }
    public decimal? PesoPorSaco { get; set; }
    public decimal? PesoTotal { get; set; }
    public decimal? MontoAdelanto { get; set; }
    public DateTime? FAdelanto { get; set; }
    public decimal? MontoTotalPago { get; set; }
    public DateTime? FPagoTotal { get; set; }
    public string? NroCuentaBancaria { get; set; }
    public string FotoDniFrontal { get; set; } = string.Empty;
    public string FotoDniPosterior { get; set; } = string.Empty;
    public string PrimeraEvidenciaFoto { get; set; } = string.Empty;
    public string SegundaEvidenciaFoto { get; set; } = string.Empty;
    public string TerceraEvidenciaFoto { get; set; } = string.Empty;
    public string EvidenciaVideo { get; set; } = string.Empty;
    public string? Estado { get; set; }
    public string? EstadoAprobacionIngeniero { get; set; }
    public string? EstadoAprobacionContadora { get; set; }
    public int? AprobadaPorIngeniero { get; set; }
    public int? AprobadaPorContadora { get; set; }
    public int? RechazadoPorIngeniero { get; set; }
    public int? RechazadoPorContadora { get; set; }
    public int? IdUsuarioAnulacion { get; set; }
    public string? MotivoAnulacion { get; set; }
    public DateTime? FAnulacion { get; set; }
    public DateTime? FAprobacionIngeniero { get; set; }
    public DateTime? FAprobacionContadora { get; set; }
    public DateTime? FRechazoIngeniero { get; set; }
    public DateTime? FRechazoContadora { get; set; }
    public DateTime FRegistro { get; set; }
    public string? Observacion { get; set; }

    // Navigation properties
    public virtual Persona? Proveedor { get; set; }
    public virtual Persona? PersonaProveedor { get; set; }
    public virtual Usuario Comisionista { get; set; } = null!;
    public virtual Usuario? AprobadaPorUsuarioIngeniero { get; set; }
    public virtual Usuario? AprobadaPorUsuarioContadora { get; set; }
    public virtual Usuario? RechazadoPorUsuarioIngeniero { get; set; }
    public virtual Usuario? RechazadoPorUsuarioContadora { get; set; }
    public virtual VariedadProducto? VariedadProducto { get; set; }
    public virtual TipoDocumento? TipoDocumento { get; set; }
    public virtual Banco? Banco { get; set; }
    public virtual Usuario? UsuarioAnulacion { get; set; }
    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}