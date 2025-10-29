using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Compras.LlegadasPlanta.Commands.CreateLlegadaPlanta;
using Miski.Application.Features.Compras.LlegadasPlanta.Queries.GetLlegadaPlantaById;
using Miski.Application.Features.Compras.LlegadasPlanta.Queries.GetCompraVehiculoConLotes;
using Miski.Application.Features.Compras.LlegadasPlanta.Queries.GetLlegadasPlanta;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Compras;

namespace Miski.Api.Controllers.Compras;

[ApiController]
[Route("api/compras/llegadas-planta")]
[Tags("Compras")]
[Authorize]
public class LlegadaPlantaController : ControllerBase
{
    private readonly IMediator _mediator;

    public LlegadaPlantaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene una llegada a planta por ID con detalles de lotes recibidos
    /// </summary>
    /// <remarks>
    /// Retorna información de la llegada a planta incluyendo:
    /// - Datos de la compra y usuario
    /// - Detalles de cada lote recibido con:
    ///   * Sacos asignados vs sacos recibidos
    ///   * Peso asignado vs peso recibido
    ///   * Diferencias calculadas
    ///   * Observaciones
    /// </remarks>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<LlegadaPlantaDto>>> GetLlegadaPlantaById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetLlegadaPlantaByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<LlegadaPlantaDto>.SuccessResult(
                result,
                "Llegada a planta obtenida exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<LlegadaPlantaDto>.ErrorResult(
                "Llegada a planta no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<LlegadaPlantaDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene las compras con sus lotes de un CompraVehiculo específico
    /// </summary>
    /// <remarks>
    /// Retorna todas las compras asignadas a un vehículo con sus lotes, indicando:
    /// - Sacos asignados y peso asignado de cada lote
    /// - Si el lote ya fue recibido (YaRecibido = true/false)
    /// - Información de recepción si existe (sacos recibidos, peso recibido, observaciones)
    /// 
    /// Útil para registrar llegadas a planta basándose en el vehículo que llega.
    /// </remarks>
    [HttpGet("por-vehiculo/{idCompraVehiculo}")]
    public async Task<ActionResult<ApiResponse<CompraVehiculoConLotesDto>>> GetCompraVehiculoConLotes(
        int idCompraVehiculo,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetCompraVehiculoConLotesQuery(idCompraVehiculo);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<CompraVehiculoConLotesDto>.SuccessResult(
                result,
                "Compras con lotes obtenidas exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<CompraVehiculoConLotesDto>.ErrorResult(
                "CompraVehiculo no encontrado",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CompraVehiculoConLotesDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Registra una nueva llegada a planta
    /// </summary>
    /// <remarks>
    /// Permite registrar la llegada de una compra a planta con los detalles de cada lote recibido.
    /// 
    /// Campos requeridos:
    /// - IdCompra: ID de la compra que llega
    /// - IdUsuario: ID del usuario que registra la llegada
    /// - FLlegada: Fecha de llegada
    /// - Detalles: Lista de lotes recibidos (mínimo 1)
    /// 
    /// Para cada detalle:
    /// - IdLote: ID del lote recibido
    /// - SacosRecibidos: Cantidad de sacos recibidos
    /// - PesoRecibido: Peso recibido en kg
    /// - Observaciones: Observaciones opcionales
    /// 
    /// Validaciones:
    /// - La compra debe existir
    /// - El usuario debe existir
    /// - Todos los lotes deben existir y pertenecer a la compra
    /// - No se permiten lotes duplicados en una misma llegada
    /// 
    /// Ejemplo de request:
    /// {
    ///   "idCompra": 1,
    ///   "idUsuario": 5,
    ///   "fLlegada": "2024-01-20T14:30:00",
    ///   "observaciones": "Llegada sin contratiempos",
    ///   "detalles": [
    ///     {
    ///       "idLote": 1,
    ///       "sacosRecibidos": 30,
    ///       "pesoRecibido": 1495.50,
    ///       "observaciones": "Algunos sacos húmedos"
    ///     },
    ///     {
    ///       "idLote": 2,
    ///       "sacosRecibidos": 40,
    ///       "pesoRecibido": 2000.00,
    ///       "observaciones": null
    ///     }
    ///   ]
    /// }
    /// </remarks>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<LlegadaPlantaDto>>> RegistrarLlegadaPlanta(
        [FromBody] CreateLlegadaPlantaDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateLlegadaPlantaCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetLlegadaPlantaById),
                new { id = result.IdLlegadaPlanta },
                ApiResponse<LlegadaPlantaDto>.SuccessResult(
                    result,
                    "Llegada a planta registrada exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<LlegadaPlantaDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<LlegadaPlantaDto>.ErrorResult(
                "Entidad relacionada no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<LlegadaPlantaDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene todas las llegadas a planta con filtros opcionales
    /// </summary>
    /// <remarks>
    /// Retorna todas las llegadas a planta con sus detalles, mostrando:
    /// - Información de la compra y usuario que registró
    /// - Detalles de cada lote con:
    ///   * Sacos asignados (del lote original)
    ///   * Sacos recibidos
    ///   * Peso asignado (del lote original)
    ///   * Peso recibido
    ///   * Diferencias calculadas (asignado - recibido)
    ///   * Observaciones
    /// 
    /// Permite filtrar por:
    /// - idCompra: Filtrar por compra específica
    /// - estado: Filtrar por estado (ej: REGISTRADO)
    /// - fechaInicio: Filtrar llegadas desde esta fecha
    /// - fechaFin: Filtrar llegadas hasta esta fecha
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<LlegadaPlantaDto>>>> GetLlegadasPlanta(
        [FromQuery] int? idCompra = null,
        [FromQuery] string? estado = null,
        [FromQuery] DateTime? fechaInicio = null,
        [FromQuery] DateTime? fechaFin = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetLlegadasPlantaQuery(idCompra, estado, fechaInicio, fechaFin);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<IEnumerable<LlegadaPlantaDto>>.SuccessResult(
                result,
                "Llegadas a planta obtenidas exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<LlegadaPlantaDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }
}