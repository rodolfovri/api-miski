using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Maestros.UnidadMedida.Commands.CreateUnidadMedida;
using Miski.Application.Features.Maestros.UnidadMedida.Commands.UpdateUnidadMedida;
using Miski.Application.Features.Maestros.UnidadMedida.Commands.DeleteUnidadMedida;
using Miski.Application.Features.Maestros.UnidadMedida.Queries.GetUnidadMedidas;
using Miski.Application.Features.Maestros.UnidadMedida.Queries.GetUnidadMedidaById;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Api.Controllers.Maestros;

[ApiController]
[Route("api/maestros/unidad-medida")]
[Tags("Maestros")]
[Authorize]
public class UnidadMedidaController : ControllerBase
{
    private readonly IMediator _mediator;

    public UnidadMedidaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todas las unidades de medida
    /// </summary>
    /// <remarks>
    /// Permite filtrar por:
    /// - nombre: Búsqueda parcial por nombre o abreviatura
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<UnidadMedidaDto>>>> GetUnidadMedidas(
        [FromQuery] string? nombre = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetUnidadMedidasQuery(nombre);
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<UnidadMedidaDto>>.SuccessResult(
                result,
                "Unidades de medida obtenidas exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<UnidadMedidaDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene una unidad de medida por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<UnidadMedidaDto>>> GetUnidadMedidaById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetUnidadMedidaByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<UnidadMedidaDto>.SuccessResult(
                result,
                "Unidad de medida obtenida exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<UnidadMedidaDto>.ErrorResult(
                "Unidad de medida no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<UnidadMedidaDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea una nueva unidad de medida
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<UnidadMedidaDto>>> CreateUnidadMedida(
        [FromBody] CreateUnidadMedidaDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateUnidadMedidaCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetUnidadMedidaById),
                new { id = result.IdUnidadMedida },
                ApiResponse<UnidadMedidaDto>.SuccessResult(
                    result,
                    "Unidad de medida creada exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<UnidadMedidaDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<UnidadMedidaDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza una unidad de medida
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<UnidadMedidaDto>>> UpdateUnidadMedida(
        int id,
        [FromBody] UpdateUnidadMedidaDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdUnidadMedida)
            {
                return BadRequest(ApiResponse<UnidadMedidaDto>.ErrorResult(
                    "ID inválido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petición"
                ));
            }

            var command = new UpdateUnidadMedidaCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<UnidadMedidaDto>.SuccessResult(
                result,
                "Unidad de medida actualizada exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<UnidadMedidaDto>.ErrorResult(
                "Unidad de medida no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<UnidadMedidaDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<UnidadMedidaDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Elimina una unidad de medida
    /// </summary>
    /// <remarks>
    /// NOTA: No se puede eliminar una unidad de medida que está siendo utilizada por productos.
    /// </remarks>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteUnidadMedida(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteUnidadMedidaCommand(id);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Unidad de medida eliminada exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Unidad de medida no encontrada",
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