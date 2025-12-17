using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Maestros.TipoMovimiento.Commands.CreateTipoMovimiento;
using Miski.Application.Features.Maestros.TipoMovimiento.Commands.DeleteTipoMovimiento;
using Miski.Application.Features.Maestros.TipoMovimiento.Commands.UpdateTipoMovimiento;
using Miski.Application.Features.Maestros.TipoMovimiento.Queries.GetTipoMovimientoById;
using Miski.Application.Features.Maestros.TipoMovimiento.Queries.GetTipoMovimientos;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Api.Controllers.Maestros;

[ApiController]
[Route("api/maestros/tipos-movimiento")]
[Tags("Maestros")]
[Authorize]
public class TipoMovimientoController : ControllerBase
{
    private readonly IMediator _mediator;

    public TipoMovimientoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los tipos de movimientos
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<TipoMovimientoDto>>>> GetTipoMovimientos(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetTipoMovimientosQuery();
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<IEnumerable<TipoMovimientoDto>>.SuccessResult(
                result,
                "Tipos de movimiento obtenidos exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<TipoMovimientoDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene un tipo de movimiento por su ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<TipoMovimientoDto>>> GetTipoMovimientoById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetTipoMovimientoByIdQuery { IdTipoMovimiento = id };
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<TipoMovimientoDto>.SuccessResult(
                result,
                "Tipo de movimiento obtenido exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<TipoMovimientoDto>.ErrorResult(
                "Tipo de movimiento no encontrado",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TipoMovimientoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea un nuevo tipo de movimiento
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<TipoMovimientoDto>>> CreateTipoMovimiento(
        [FromBody] CreateTipoMovimientoDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateTipoMovimientoCommand { TipoMovimientoData = request };
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetTipoMovimientoById),
                new { id = result.IdTipoMovimiento },
                ApiResponse<TipoMovimientoDto>.SuccessResult(
                    result,
                    "Tipo de movimiento creado exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<TipoMovimientoDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TipoMovimientoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza un tipo de movimiento existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<TipoMovimientoDto>>> UpdateTipoMovimiento(
        int id,
        [FromBody] UpdateTipoMovimientoDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdTipoMovimiento)
            {
                return BadRequest(ApiResponse<TipoMovimientoDto>.ErrorResult(
                    "ID inválido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petición"
                ));
            }

            var command = new UpdateTipoMovimientoCommand { TipoMovimientoData = request };
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<TipoMovimientoDto>.SuccessResult(
                result,
                "Tipo de movimiento actualizado exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<TipoMovimientoDto>.ErrorResult(
                "Tipo de movimiento no encontrado",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<TipoMovimientoDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TipoMovimientoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Elimina un tipo de movimiento
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteTipoMovimiento(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteTipoMovimientoCommand { IdTipoMovimiento = id };
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Tipo de movimiento eliminado exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Tipo de movimiento no encontrado",
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
