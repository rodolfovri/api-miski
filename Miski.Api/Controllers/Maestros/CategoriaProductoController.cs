using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Maestros.CategoriaProducto.Commands.CreateCategoria;
using Miski.Application.Features.Maestros.CategoriaProducto.Commands.UpdateCategoria;
using Miski.Application.Features.Maestros.CategoriaProducto.Commands.DeleteCategoria;
using Miski.Application.Features.Maestros.CategoriaProducto.Queries.GetCategorias;
using Miski.Application.Features.Maestros.CategoriaProducto.Queries.GetCategoriaById;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Api.Controllers.Maestros;

[ApiController]
[Route("api/maestros/categoria-producto")]
[Tags("Maestros")]
[Authorize]
public class CategoriaProductoController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriaProductoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todas las categor�as de productos
    /// </summary>
    /// <remarks>
    /// Permite filtrar por:
    /// - nombre: B�squeda parcial por nombre
    /// - estado: Filtrar por estado (ACTIVO, INACTIVO)
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoriaProductoDto>>>> GetCategorias(
        [FromQuery] string? nombre = null,
        [FromQuery] string? estado = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetCategoriasProductoQuery(nombre, estado);
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<CategoriaProductoDto>>.SuccessResult(
                result,
                "Categor�as obtenidas exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<CategoriaProductoDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene una categor�a por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CategoriaProductoDto>>> GetCategoriaById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetCategoriaProductoByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<CategoriaProductoDto>.SuccessResult(
                result,
                "Categor�a obtenida exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<CategoriaProductoDto>.ErrorResult(
                "Categor�a no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CategoriaProductoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea una nueva categor�a de producto
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CategoriaProductoDto>>> CreateCategoria(
        [FromBody] CreateCategoriaProductoDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateCategoriaProductoCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetCategoriaById),
                new { id = result.IdCategoriaProducto },
                ApiResponse<CategoriaProductoDto>.SuccessResult(
                    result,
                    "Categor�a creada exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<CategoriaProductoDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CategoriaProductoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza una categor�a de producto
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CategoriaProductoDto>>> UpdateCategoria(
        int id,
        [FromBody] UpdateCategoriaProductoDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdCategoriaProducto)
            {
                return BadRequest(ApiResponse<CategoriaProductoDto>.ErrorResult(
                    "ID inv�lido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petici�n"
                ));
            }

            var command = new UpdateCategoriaProductoCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<CategoriaProductoDto>.SuccessResult(
                result,
                "Categor�a actualizada exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<CategoriaProductoDto>.ErrorResult(
                "Categor�a no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<CategoriaProductoDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CategoriaProductoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Elimina una categor�a de producto (cambio de estado a INACTIVO)
    /// </summary>
    /// <remarks>
    /// NOTA: No se puede eliminar una categor�a que tiene productos asociados.
    /// </remarks>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteCategoria(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteCategoriaProductoCommand(id);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Categor�a eliminada exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Categor�a no encontrada",
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