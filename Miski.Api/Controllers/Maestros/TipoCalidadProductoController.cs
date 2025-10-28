using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Maestros.TipoCalidadProducto.Commands.CreateTipoCalidadProducto;
using Miski.Application.Features.Maestros.TipoCalidadProducto.Commands.UpdateTipoCalidadProducto;
using Miski.Application.Features.Maestros.TipoCalidadProducto.Commands.DeleteTipoCalidadProducto;
using Miski.Application.Features.Maestros.TipoCalidadProducto.Queries.GetTipoCalidadProductos;
using Miski.Application.Features.Maestros.TipoCalidadProducto.Queries.GetTipoCalidadProductoById;
using Miski.Application.Features.Maestros.TipoCalidadProducto.Queries.GetTipoCalidadByProductoId;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Api.Controllers.Maestros;

[ApiController]
[Route("api/maestros/tipo-calidad-producto")]
[Tags("Maestros")]
[Authorize]
public class TipoCalidadProductoController : ControllerBase
{
    private readonly IMediator _mediator;

    public TipoCalidadProductoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los tipos de calidad de producto
    /// </summary>
    /// <param name="idProducto">Filtrar por producto (opcional)</param>
    /// <param name="estado">Filtrar por estado (opcional)</param>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<TipoCalidadProductoDto>>>> GetTipoCalidadProductos(
        [FromQuery] int? idProducto = null,
        [FromQuery] string? estado = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetTipoCalidadProductosQuery(idProducto, estado);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<IEnumerable<TipoCalidadProductoDto>>.SuccessResult(
                result,
                "Tipos de calidad obtenidos exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<TipoCalidadProductoDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene un tipo de calidad por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<TipoCalidadProductoDto>>> GetTipoCalidadProductoById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetTipoCalidadProductoByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<TipoCalidadProductoDto>.SuccessResult(
                result,
                "Tipo de calidad obtenido exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<TipoCalidadProductoDto>.ErrorResult(
                "Tipo de calidad no encontrado",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TipoCalidadProductoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea un nuevo tipo de calidad de producto
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<TipoCalidadProductoDto>>> CreateTipoCalidadProducto(
        [FromBody] CreateTipoCalidadProductoDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateTipoCalidadProductoCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetTipoCalidadProductoById),
                new { id = result.IdTipoCalidadProducto },
                ApiResponse<TipoCalidadProductoDto>.SuccessResult(
                    result,
                    "Tipo de calidad creado exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<TipoCalidadProductoDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<TipoCalidadProductoDto>.ErrorResult(
                "Producto no encontrado",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TipoCalidadProductoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza un tipo de calidad de producto
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<TipoCalidadProductoDto>>> UpdateTipoCalidadProducto(
        int id,
        [FromBody] UpdateTipoCalidadProductoDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdTipoCalidadProducto)
            {
                return BadRequest(ApiResponse<TipoCalidadProductoDto>.ErrorResult(
                    "ID inválido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petición"
                ));
            }

            var command = new UpdateTipoCalidadProductoCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<TipoCalidadProductoDto>.SuccessResult(
                result,
                "Tipo de calidad actualizado exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<TipoCalidadProductoDto>.ErrorResult(
                "Tipo de calidad no encontrado",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<TipoCalidadProductoDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TipoCalidadProductoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Elimina (inactiva) un tipo de calidad de producto
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteTipoCalidadProducto(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteTipoCalidadProductoCommand(id);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Tipo de calidad eliminado exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Tipo de calidad no encontrado",
                ex.Message
            ));
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
    /// Obtiene todas las calidades relacionadas con un producto específico
    /// </summary>
    /// <param name="idProducto">ID del producto</param>
    [HttpGet("producto/{idProducto}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<TipoCalidadProductoDto>>>> GetTipoCalidadByProductoId(
        int idProducto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetTipoCalidadByProductoIdQuery(idProducto);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<IEnumerable<TipoCalidadProductoDto>>.SuccessResult(
                result,
                $"Calidades del producto obtenidas exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<IEnumerable<TipoCalidadProductoDto>>.ErrorResult(
                "Producto no encontrado",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<TipoCalidadProductoDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }
}
