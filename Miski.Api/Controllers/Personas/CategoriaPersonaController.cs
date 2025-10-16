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
    /// Obtiene todas las categor�as de personas
    /// </summary>
    /// <remarks>
    /// Permite filtrar por:
    /// - nombre: B�squeda parcial por nombre de la categor�a
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
                "Categor�as obtenidas exitosamente"
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
    /// Obtiene una categor�a por ID
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
                "Categor�a obtenida exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<CategoriaPersonaDto>.ErrorResult(
                "Categor�a no encontrada",
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
    /// Crea una nueva categor�a
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
                    "Categor�a creada exitosamente"
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
    /// Actualiza una categor�a
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
                    "ID inv�lido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petici�n"
                ));
            }

            var command = new UpdateCategoriaPersonaCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<CategoriaPersonaDto>.SuccessResult(
                result,
                "Categor�a actualizada exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<CategoriaPersonaDto>.ErrorResult(
                "Categor�a no encontrada",
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
    /// Elimina una categor�a
    /// </summary>
    /// <remarks>
    /// NOTA: No se puede eliminar una categor�a que tiene personas asociadas.
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