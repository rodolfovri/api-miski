using Microsoft.AspNetCore.Http;

namespace Miski.Shared.DTOs.Compras;

public class NegociacionDto
{
    public int IdNegociacion { get; set; }
    public int IdComisionista { get; set; }
    public int? IdTipoDocumento { get; set; }  // AGREGADO
    public int? IdBanco { get; set; }  // AGREGADO
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
    public DateTime? FAprobacionIngeniero { get; set; }
    public DateTime? FAprobacionContadora { get; set; }
    public DateTime? FRechazoIngeniero { get; set; }
    public DateTime? FRechazoContadora { get; set; }
    public DateTime FRegistro { get; set; }
    public string? Observacion { get; set; }
    
    // Información adicional
    public string? ProveedorNombre { get; set; }
    public string? ComisionistaNombre { get; set; }
    public int? IdProducto { get; set; }  // ? AGREGADO - ID del producto desde VariedadProducto
    public string? VariedadProductoNombre { get; set; }
    public string? ProductoNombre { get; set; }
    public string? TipoDocumentoNombre { get; set; }  // AGREGADO
    public string? BancoNombre { get; set; }  // AGREGADO
    public string? AprobadaPorIngenieroNombre { get; set; }
    public string? AprobadaPorContadoraNombre { get; set; }
}

// DTO para crear negociación - PRIMERA ETAPA
public class CreateNegociacionDto
{
    public int IdComisionista { get; set; }
    public int? IdVariedadProducto { get; set; }
    public int? SacosTotales { get; set; }
    public string TipoCalidad { get; set; } = string.Empty;
    public decimal? PrecioUnitario { get; set; }
    // Estado se asigna automáticamente como 'EN PROCESO'
}

// DTO para completar negociación - SEGUNDA ETAPA
public class CompletarNegociacionDto
{
    public int IdNegociacion { get; set; }
    public int? IdTipoDocumento { get; set; }  // AGREGADO
    public int? IdBanco { get; set; }  // AGREGADO
    public string NroDocumentoProveedor { get; set; } = string.Empty;
    public string NroCuentaBancaria { get; set; } = string.Empty;
    
    // Fotos y video (obligatorias en primera completación, opcionales en reenvío)
    public IFormFile? FotoDniFrontal { get; set; }
    public IFormFile? FotoDniPosterior { get; set; }
    public IFormFile? PrimeraEvidenciaFoto { get; set; }
    public IFormFile? SegundaEvidenciaFoto { get; set; }
    public IFormFile? TerceraEvidenciaFoto { get; set; }
    public IFormFile? EvidenciaVideo { get; set; }
}

// DTO para actualizar negociación - PRIMERA ETAPA (igual que CREATE)
public class UpdateNegociacionDto
{
    public int IdNegociacion { get; set; }
    public int IdComisionista { get; set; }
    public int? IdVariedadProducto { get; set; }
    public int? SacosTotales { get; set; }
    public string TipoCalidad { get; set; } = string.Empty;
    public decimal? PrecioUnitario { get; set; }
    // Estado se mantiene en 'EN PROCESO' y EstadoAprobacionIngeniero en 'PENDIENTE'
}

// DTO para aprobar negociación por Ingeniero
public class AprobarNegociacionIngenieroDto
{
    public int IdNegociacion { get; set; }
    public int AprobadaPorIngeniero { get; set; }
}

// DTO para rechazar negociación por Ingeniero
public class RechazarNegociacionIngenieroDto
{
    public int IdNegociacion { get; set; }
    public int RechazadoPorIngeniero { get; set; }
}

// DTO para aprobar negociación por Contadora - SEGUNDA ETAPA
public class AprobarNegociacionContadoraDto
{
    public int IdNegociacion { get; set; }
    public int AprobadaPorContadora { get; set; }
}

// DTO para rechazar negociación por Contadora - SEGUNDA ETAPA
public class RechazarNegociacionContadoraDto
{
    public int IdNegociacion { get; set; }
    public int RechazadoPorContadora { get; set; }
}

// DTO para anular negociación
public class AnularNegociacionDto
{
    public int IdUsuarioAnulacion { get; set; }
    public string MotivoAnulacion { get; set; } = string.Empty;
}
