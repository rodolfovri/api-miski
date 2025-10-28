using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Ubicaciones.Commands.CreateUbicacion;
using Miski.Application.Features.Ubicaciones.Commands.UpdateUbicacion;
using Miski.Application.Features.Ubicaciones.Commands.DeleteUbicacion;
using Miski.Application.Features.Ubicaciones.Queries.GetUbicaciones;
using Miski.Application.Features.Ubicaciones.Queries.GetUbicacionById;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Ubicaciones;

namespace Miski.Api.Controllers.Ubicaciones;

[ApiController]
[Route("api/ubicaciones")]
[Tags("Ubicaciones")]
[Authorize]
public class UbicacionesController : ControllerBase
{
    private readonly IMediator _mediator;

    public UbicacionesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todas las ubicaciones
    /// </summary>
    /// <remarks>
    /// Permite filtrar por:
    /// - nombre: Búsqueda parcial por nombre de la ubicación
    /// - tipo: Filtrar por tipo de ubicación
    /// - estado: Filtrar por estado (ACTIVO, INACTIVO)
    /// - idUsuario: Filtrar por usuario propietario
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<UbicacionDto>>>> GetUbicaciones(
        [FromQuery] string? nombre = null,
        [FromQuery] string? tipo = null,
        [FromQuery] string? estado = null,
        [FromQuery] int? idUsuario = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetUbicacionesQuery(nombre, tipo, estado, idUsuario);
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<UbicacionDto>>.SuccessResult(
                result,
                "Ubicaciones obtenidas exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<UbicacionDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene una ubicación por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<UbicacionDto>>> GetUbicacionById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetUbicacionByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<UbicacionDto>.SuccessResult(
                result,
                "Ubicación obtenida exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<UbicacionDto>.ErrorResult(
                "Ubicación no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<UbicacionDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea una nueva ubicación
    /// </summary>
    /// <remarks>
    /// NOTA: Este endpoint usa multipart/form-data porque acepta un archivo PDF opcional (ComprobantePdf).
    /// </remarks>
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ApiResponse<UbicacionDto>>> CreateUbicacion(
        [FromForm] CreateUbicacionDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateUbicacionCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetUbicacionById),
                new { id = result.IdUbicacion },
                ApiResponse<UbicacionDto>.SuccessResult(
                    result,
                    "Ubicación creada exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return BadRequest(ApiResponse<UbicacionDto>.ErrorResult(
                "Datos no encontrados",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<UbicacionDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<UbicacionDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza una ubicación
    /// </summary>
    /// <remarks>
    /// NOTA: Este endpoint usa multipart/form-data porque acepta un archivo PDF opcional (ComprobantePdf).
    /// Si no se envía un nuevo PDF, se mantiene el anterior.
    /// </remarks>
    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ApiResponse<UbicacionDto>>> UpdateUbicacion(
        int id,
        [FromForm] UpdateUbicacionDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdUbicacion)
            {
                return BadRequest(ApiResponse<UbicacionDto>.ErrorResult(
                    "ID inválido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petición"
                ));
            }

            var command = new UpdateUbicacionCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<UbicacionDto>.SuccessResult(
                result,
                "Ubicación actualizada exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<UbicacionDto>.ErrorResult(
                "Ubicación no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<UbicacionDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<UbicacionDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Elimina una ubicación (cambio de estado a INACTIVO)
    /// </summary>
    /// <remarks>
    /// NOTA: No se puede eliminar una ubicación que tiene stock asociado.
    /// </remarks>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteUbicacion(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteUbicacionCommand(id);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Ubicación eliminada exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Ubicación no encontrada",
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
}