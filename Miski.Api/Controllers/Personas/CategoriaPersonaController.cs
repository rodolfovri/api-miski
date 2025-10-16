using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Personas.CategoriaPersona.Commands.CreateCategoria;
using Miski.Application.Features.Personas.CategoriaPersona.Commands.DeleteCategoria;
using Miski.Application.Features.Personas.CategoriaPersona.Commands.UpdateCategoria;
using Miski.Application.Features.Personas.CategoriaPersona.Queries.GetCategorias;
using Miski.Application.Features.Personas.CategoriaPersona.Queries.GetCategoriaById;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Personas;

namespace Miski.Api.Controllers.Personas;

[ApiController]
[Route("api/personas/categorias")]
[Tags("Personas")]
[Authorize]
public class CategoriaPersonaController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriaPersonaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todas las categorías de personas
    /// </summary>
    /// <remarks>
    /// Permite filtrar por:
    /// - nombre: Búsqueda parcial por nombre de la categoría
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoriaPersonaDto>>>> GetCategorias(
        [FromQuery] string? nombre = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetCategoriasQuery(nombre);
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<CategoriaPersonaDto>>.SuccessResult(
                result,
                "Categorías obtenidas exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<CategoriaPersonaDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene una categoría por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CategoriaPersonaDto>>> GetCategoriaById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetCategoriaByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<CategoriaPersonaDto>.SuccessResult(
                result,
                "Categoría obtenida exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<CategoriaPersonaDto>.ErrorResult(
                "Categoría no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CategoriaPersonaDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea una nueva categoría
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CategoriaPersonaDto>>> CreateCategoria(
        [FromBody] CreateCategoriaPersonaDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateCategoriaPersonaCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetCategoriaById),
                new { id = result.IdCategoriaPersona },
                ApiResponse<CategoriaPersonaDto>.SuccessResult(
                    result,
                    "Categoría creada exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<CategoriaPersonaDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CategoriaPersonaDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza una categoría
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CategoriaPersonaDto>>> UpdateCategoria(
        int id,
        [FromBody] UpdateCategoriaPersonaDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdCategoriaPersona)
            {
                return BadRequest(ApiResponse<CategoriaPersonaDto>.ErrorResult(
                    "ID inválido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petición"
                ));
            }

            var command = new UpdateCategoriaPersonaCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<CategoriaPersonaDto>.SuccessResult(
                result,
                "Categoría actualizada exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<CategoriaPersonaDto>.ErrorResult(
                "Categoría no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<CategoriaPersonaDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CategoriaPersonaDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Elimina una categoría
    /// </summary>
    /// <remarks>
    /// NOTA: No se puede eliminar una categoría que tiene personas asociadas.
    /// </remarks>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteCategoria(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteCategoriaPersonaCommand(id);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Categoría eliminada exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Categoría no encontrada",
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