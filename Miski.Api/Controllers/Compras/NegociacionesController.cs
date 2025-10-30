using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Compras.Negociaciones.Commands.CreateNegociacion;
using Miski.Application.Features.Compras.Negociaciones.Commands.UpdateNegociacion;
using Miski.Application.Features.Compras.Negociaciones.Commands.DeleteNegociacion;
using Miski.Application.Features.Compras.Negociaciones.Commands.CompletarNegociacion;
using Miski.Application.Features.Compras.Negociaciones.Commands.AprobarNegociacionIngeniero;
using Miski.Application.Features.Compras.Negociaciones.Commands.RechazarNegociacionIngeniero;
using Miski.Application.Features.Compras.Negociaciones.Commands.AprobarNegociacionContadora;
using Miski.Application.Features.Compras.Negociaciones.Commands.RechazarNegociacionContadora;
using Miski.Application.Features.Compras.Negociaciones.Queries.GetNegociaciones;
using Miski.Application.Features.Compras.Negociaciones.Queries.GetNegociacionById;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Compras;

namespace Miski.Api.Controllers.Compras;

[ApiController]
[Route("api/compras/negociaciones")]
[Tags("Compras")]
[Authorize]
public class NegociacionesController : ControllerBase
{
    private readonly IMediator _mediator;

    public NegociacionesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todas las negociaciones con filtros opcionales
    /// </summary>
    /// <remarks>
    /// Permite filtrar por:
    /// - idProveedor: Filtrar por proveedor
    /// - idComisionista: Filtrar por comisionista
    /// - idProducto: Filtrar por producto
    /// - estadoAprobado: Filtrar por estado de aprobaci�n (PENDIENTE/APROBADO/RECHAZADO)
    /// - estado: Filtrar por estado general (ACTIVO/INACTIVO)
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<NegociacionDto>>>> GetNegociaciones(
        [FromQuery] int? idComisionista = null,
        [FromQuery] int? idVariedadProducto = null,  // CAMBIADO de idProducto
        [FromQuery] string? estado = null,
        [FromQuery] string? estadoAprobacionIngeniero = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetNegociacionesQuery(idComisionista, idVariedadProducto, estado, estadoAprobacionIngeniero);
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<NegociacionDto>>.SuccessResult(
                result, 
                "Negociaciones obtenidas exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<NegociacionDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene una negociaci�n por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<NegociacionDto>>> GetNegociacionById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetNegociacionByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<NegociacionDto>.SuccessResult(
                result,
                "Negociaci�n obtenida exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<NegociacionDto>.ErrorResult(
                "Negociaci�n no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<NegociacionDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea una nueva negociaci�n - PRIMERA ETAPA
    /// </summary>
    /// <remarks>
    /// Solo requiere: IdComisionista, SacosTotales, TipoCalidad, PrecioUnitario
    /// El estado se asigna autom�ticamente como 'EN PROCESO'
    /// </remarks>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<NegociacionDto>>> CreateNegociacion(
        [FromBody] CreateNegociacionDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateNegociacionCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetNegociacionById), 
                new { id = result.IdNegociacion }, 
                ApiResponse<NegociacionDto>.SuccessResult(
                    result, 
                    "Negociaci�n creada exitosamente con estado 'EN PROCESO'"
                )
            );
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<NegociacionDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<NegociacionDto>.ErrorResult(
                "Entidad relacionada no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<NegociacionDto>.ErrorResult(
                "Error interno del servidor", 
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza una negociaci�n - PRIMERA ETAPA
    /// </summary>
    /// <remarks>
    /// Solo permite actualizar negociaciones en estado 'EN PROCESO' con EstadoAprobacionIngeniero 'PENDIENTE'.
    /// Acepta los mismos par�metros que el POST: IdComisionista, IdVariedadProducto, SacosTotales, TipoCalidad, PrecioUnitario.
    /// Calcula autom�ticamente: PesoPorSaco (50kg), PesoTotal y MontoTotalPago.
    /// Mantiene el estado en 'EN PROCESO' y EstadoAprobacionIngeniero en 'PENDIENTE'.
    /// </remarks>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<NegociacionDto>>> UpdateNegociacion(
        int id,
        [FromBody] UpdateNegociacionDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdNegociacion)
            {
                return BadRequest(ApiResponse<NegociacionDto>.ErrorResult(
                    "ID inv�lido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petici�n"
                ));
            }

            var command = new UpdateNegociacionCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<NegociacionDto>.SuccessResult(
                result,
                "Negociaci�n actualizada exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<NegociacionDto>.ErrorResult(
                "Negociaci�n no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<NegociacionDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<NegociacionDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Anula una negociaci�n
    /// </summary>
    /// <remarks>
    /// NOTA: Solo se pueden anular negociaciones en los siguientes estados:
    /// 1. Estado EN PROCESO y EstadoAprobacionIngeniero PENDIENTE
    /// 2. Estado APROBADO y EstadoAprobacionIngeniero APROBADO
    /// 3. Estado EN REVISI�N y EstadoAprobacionContadora PENDIENTE
    /// 
    /// Los campos Estado, EstadoAprobacionIngeniero y EstadoAprobacionContadora se cambiar�n a ANULADO.
    /// Se debe proporcionar IdUsuarioAnulacion y MotivoAnulacion.
    /// </remarks>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteNegociacion(
        int id,
        [FromBody] AnularNegociacionDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteNegociacionCommand(id, request.IdUsuarioAnulacion, request.MotivoAnulacion);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Negociaci�n anulada exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Negociaci�n no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene negociaciones pendientes de aprobaci�n por ingeniero
    /// </summary>
    [HttpGet("pendientes-ingeniero")]
    public async Task<ActionResult<ApiResponse<IEnumerable<NegociacionDto>>>> GetPendientesIngeniero(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetNegociacionesQuery(EstadoAprobacionIngeniero: "PENDIENTE");
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<NegociacionDto>>.SuccessResult(
                result, 
                "Negociaciones pendientes obtenidas exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<NegociacionDto>>.ErrorResult(
                "Error al obtener negociaciones pendientes", 
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Aprueba una negociaci�n por parte del ingeniero - PRIMERA ETAPA
    /// </summary>
    /// <remarks>
    /// Cambia EstadoAprobacionIngeniero a 'APROBADO' y Estado a 'APROBADO'
    /// Solo si est� en estado 'EN PROCESO' y EstadoAprobacionIngeniero 'PENDIENTE'
    /// </remarks>
    [HttpPut("{id}/aprobar-ingeniero")]
    public async Task<ActionResult<ApiResponse<NegociacionDto>>> AprobarPorIngeniero(
        int id,
        [FromBody] AprobarNegociacionIngenieroDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdNegociacion)
            {
                return BadRequest(ApiResponse<NegociacionDto>.ErrorResult(
                    "ID inv�lido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petici�n"
                ));
            }

            var command = new AprobarNegociacionIngenieroCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<NegociacionDto>.SuccessResult(
                result,
                "Negociaci�n aprobada por ingeniero exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<NegociacionDto>.ErrorResult(
                "Negociaci�n no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<NegociacionDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<NegociacionDto>.ErrorResult(
                "Error al aprobar la negociaci�n", 
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Rechaza una negociaci�n por parte del ingeniero - PRIMERA ETAPA
    /// </summary>
    /// <remarks>
    /// Cambia EstadoAprobacionIngeniero a 'RECHAZADO' y Estado a 'ANULADO'
    /// Solo si est� en estado 'EN PROCESO' y EstadoAprobacionIngeniero 'PENDIENTE'
    /// </remarks>
    [HttpPut("{id}/rechazar-ingeniero")]
    public async Task<ActionResult<ApiResponse<NegociacionDto>>> RechazarPorIngeniero(
        int id,
        [FromBody] RechazarNegociacionIngenieroDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdNegociacion)
            {
                return BadRequest(ApiResponse<NegociacionDto>.ErrorResult(
                    "ID inv�lido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petici�n"
                ));
            }

            var command = new RechazarNegociacionIngenieroCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<NegociacionDto>.SuccessResult(
                result,
                "Negociaci�n rechazada por ingeniero exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<NegociacionDto>.ErrorResult(
                "Negociaci�n no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<NegociacionDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<NegociacionDto>.ErrorResult(
                "Error al rechazar la negociaci�n", 
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Completa una negociaci�n con datos del proveedor y evidencias - SEGUNDA ETAPA
    /// </summary>
    /// <remarks>
    /// NOTA: Este endpoint acepta multipart/form-data para subir fotos y video.
    /// Solo se puede completar si est� APROBADO por el ingeniero.
    /// Campos requeridos:
    /// - NroDocumentoProveedor
    /// - NroCuentaBancaria
    /// - FotoDniFrontal (archivo)
    /// - FotoDniPosterior (archivo)
    /// - PrimeraEvindenciaFoto (archivo)
    /// - SegundaEvindenciaFoto (archivo)
    /// - TerceraEvindenciaFoto (archivo)
    /// - EvidenciaVideo (archivo de video)
    /// 
    /// Campos opcionales:
    /// - IdTipoDocumento (tipo de documento del proveedor)
    /// - IdBanco (banco de la cuenta)
    /// 
    /// Despu�s de completar, el estado cambia a 'EN REVISI�N'
    /// </remarks>
    [HttpPut("{id}/completar")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ApiResponse<NegociacionDto>>> CompletarNegociacion(
        int id,
        [FromForm] CompletarNegociacionDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdNegociacion)
            {
                return BadRequest(ApiResponse<NegociacionDto>.ErrorResult(
                    "ID inv�lido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petici�n"
                ));
            }

            var command = new CompletarNegociacionCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<NegociacionDto>.SuccessResult(
                result,
                "Negociaci�n completada exitosamente. Estado cambiado a 'EN REVISI�N'"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<NegociacionDto>.ErrorResult(
                "Negociaci�n no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<NegociacionDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<NegociacionDto>.ErrorResult(
                "Error al completar la negociaci�n", 
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene negociaciones pendientes de aprobaci�n por contadora
    /// </summary>
    [HttpGet("pendientes-contadora")]
    public async Task<ActionResult<ApiResponse<IEnumerable<NegociacionDto>>>> GetPendientesContadora(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetNegociacionesQuery(Estado: "EN REVISI�N");
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<NegociacionDto>>.SuccessResult(
                result, 
                "Negociaciones pendientes de contadora obtenidas exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<NegociacionDto>>.ErrorResult(
                "Error al obtener negociaciones pendientes", 
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Aprueba una negociaci�n por parte de la contadora - SEGUNDA ETAPA
    /// </summary>
    /// <remarks>
    /// Cambia EstadoAprobacionContadora a 'APROBADO' y Estado a 'FINALIZADO'
    /// Solo si est� en estado 'EN REVISI�N' y EstadoAprobacionContadora 'PENDIENTE'
    /// </remarks>
    [HttpPut("{id}/aprobar-contadora")]
    public async Task<ActionResult<ApiResponse<NegociacionDto>>> AprobarPorContadora(
        int id,
        [FromBody] AprobarNegociacionContadoraDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdNegociacion)
            {
                return BadRequest(ApiResponse<NegociacionDto>.ErrorResult(
                    "ID inv�lido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petici�n"
                ));
            }

            var command = new AprobarNegociacionContadoraCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<NegociacionDto>.SuccessResult(
                result,
                "Negociaci�n aprobada por contadora exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<NegociacionDto>.ErrorResult(
                "Negociaci�n no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<NegociacionDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<NegociacionDto>.ErrorResult(
                "Error al aprobar la negociaci�n", 
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Rechaza una negociaci�n por parte de la contadora - SEGUNDA ETAPA
    /// </summary>
    /// <remarks>
    /// Cambia EstadoAprobacionContadora a 'RECHAZADO' y Estado a 'ANULADO'
    /// Solo si est� en estado 'EN REVISI�N' y EstadoAprobacionContadora 'PENDIENTE'
    /// </remarks>
    [HttpPut("{id}/rechazar-contadora")]
    public async Task<ActionResult<ApiResponse<NegociacionDto>>> RechazarPorContadora(
        int id,
        [FromBody] RechazarNegociacionContadoraDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdNegociacion)
            {
                return BadRequest(ApiResponse<NegociacionDto>.ErrorResult(
                    "ID inv�lido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petici�n"
                ));
            }

            var command = new RechazarNegociacionContadoraCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<NegociacionDto>.SuccessResult(
                result,
                "Negociaci�n rechazada por contadora exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<NegociacionDto>.ErrorResult(
                "Negociaci�n no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<NegociacionDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<NegociacionDto>.ErrorResult(
                "Error al rechazar la negociaci�n", 
                ex.Message
            ));
        }
    }
}