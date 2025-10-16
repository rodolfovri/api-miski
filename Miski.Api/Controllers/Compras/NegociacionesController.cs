using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Compras.Negociaciones.Commands.CreateNegociacion;
using Miski.Application.Features.Compras.Negociaciones.Commands.UpdateNegociacion;
using Miski.Application.Features.Compras.Negociaciones.Commands.DeleteNegociacion;
using Miski.Application.Features.Compras.Negociaciones.Commands.AprobarNegociacion;
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
        [FromQuery] int? idProveedor = null,
        [FromQuery] int? idComisionista = null,
        [FromQuery] int? idProducto = null,
        [FromQuery] string? estadoAprobado = null,
        [FromQuery] string? estado = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetNegociacionesQuery(idProveedor, idComisionista, idProducto, estadoAprobado, estado);
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
    /// Crea una nueva negociaci�n
    /// </summary>
    /// <remarks>
    /// NOTA: Este endpoint acepta multipart/form-data para subir las fotos.
    /// Las fotos son obligatorias:
    /// - FotoCalidadProducto
    /// - FotoDniFrontal
    /// - FotoDniPosterior
    /// </remarks>
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ApiResponse<NegociacionDto>>> CreateNegociacion(
        [FromForm] CreateNegociacionDto request,
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
                    "Negociaci�n creada exitosamente"
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
    /// Actualiza una negociaci�n
    /// </summary>
    /// <remarks>
    /// NOTA: Este endpoint acepta multipart/form-data.
    /// Las fotos son opcionales, solo se actualizan si se env�an.
    /// No se puede actualizar una negociaci�n ya aprobada.
    /// </remarks>
    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ApiResponse<NegociacionDto>>> UpdateNegociacion(
        int id,
        [FromForm] UpdateNegociacionDto request,
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
    /// Elimina (inactiva) una negociaci�n
    /// </summary>
    /// <remarks>
    /// NOTA: No se puede eliminar una negociaci�n que tiene compras asociadas.
    /// El registro no se elimina f�sicamente, solo se marca como INACTIVO.
    /// </remarks>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteNegociacion(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteNegociacionCommand(id);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Negociaci�n eliminada exitosamente"));
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
    /// Obtiene negociaciones pendientes de aprobaci�n
    /// </summary>
    [HttpGet("pendientes-aprobacion")]
    public async Task<ActionResult<ApiResponse<IEnumerable<NegociacionDto>>>> GetPendientesAprobacion(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetNegociacionesQuery(EstadoAprobado: "PENDIENTE");
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
    /// Aprueba una negociaci�n
    /// </summary>
    [HttpPut("{id}/aprobar")]
    public async Task<ActionResult<ApiResponse<NegociacionDto>>> AprobarNegociacion(
        int id,
        [FromBody] AprobarNegociacionDto request,
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

            var command = new AprobarNegociacionCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<NegociacionDto>.SuccessResult(
                result,
                "Negociaci�n aprobada exitosamente"
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
}