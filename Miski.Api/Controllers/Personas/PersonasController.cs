using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Personas.Commands.CreatePersona;
using Miski.Application.Features.Personas.Commands.UpdatePersona;
using Miski.Application.Features.Personas.Commands.DeletePersona;
using Miski.Application.Features.Personas.Commands.AsignarCategoria;
using Miski.Application.Features.Personas.Commands.RemoverCategoria;
using Miski.Application.Features.Personas.Queries.GetPersonas;
using Miski.Application.Features.Personas.Queries.GetPersonaById;
using Miski.Application.Features.Personas.Queries.GetCategoriasByPersona;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Personas;

namespace Miski.Api.Controllers.Personas;

[ApiController]
[Route("api/personas")]
[Tags("Personas")]
[Authorize]
public class PersonasController : ControllerBase
{
    private readonly IMediator _mediator;

    public PersonasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todas las personas
    /// </summary>
    /// <remarks>
    /// Permite filtrar por:
    /// - numeroDocumento: Búsqueda parcial por número de documento
    /// - nombres: Búsqueda parcial por nombres o apellidos
    /// - estado: Filtrar por estado (ACTIVO, INACTIVO)
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<PersonaDto>>>> GetPersonas(
        [FromQuery] string? numeroDocumento = null,
        [FromQuery] string? nombres = null,
        [FromQuery] string? estado = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetPersonasQuery(numeroDocumento, nombres, estado);
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<PersonaDto>>.SuccessResult(
                result,
                "Personas obtenidas exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<PersonaDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene una persona por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PersonaDto>>> GetPersonaById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetPersonaByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<PersonaDto>.SuccessResult(
                result,
                "Persona obtenida exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<PersonaDto>.ErrorResult(
                "Persona no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<PersonaDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea una nueva persona
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<PersonaDto>>> CreatePersona(
        [FromBody] CreatePersonaDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreatePersonaCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetPersonaById),
                new { id = result.IdPersona },
                ApiResponse<PersonaDto>.SuccessResult(
                    result,
                    "Persona creada exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return BadRequest(ApiResponse<PersonaDto>.ErrorResult(
                "Datos no encontrados",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<PersonaDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<PersonaDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza una persona
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<PersonaDto>>> UpdatePersona(
        int id,
        [FromBody] UpdatePersonaDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdPersona)
            {
                return BadRequest(ApiResponse<PersonaDto>.ErrorResult(
                    "ID inválido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petición"
                ));
            }

            var command = new UpdatePersonaCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<PersonaDto>.SuccessResult(
                result,
                "Persona actualizada exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<PersonaDto>.ErrorResult(
                "Persona no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<PersonaDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<PersonaDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Elimina una persona (cambio de estado a INACTIVO)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeletePersona(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeletePersonaCommand(id);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Persona eliminada exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Persona no encontrada",
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
    /// Obtiene las categorías de una persona
    /// </summary>
    [HttpGet("{id}/categorias")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoriaPersonaDto>>>> GetCategoriasByPersona(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetCategoriasByPersonaQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<CategoriaPersonaDto>>.SuccessResult(
                result,
                "Categorías de la persona obtenidas exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<IEnumerable<CategoriaPersonaDto>>.ErrorResult(
                "Persona no encontrada",
                ex.Message
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
    /// Asigna una categoría a una persona
    /// </summary>
    [HttpPost("{personaId}/categorias/{categoriaId}")]
    public async Task<ActionResult<ApiResponse>> AsignarCategoria(
        int personaId,
        int categoriaId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new AsignarCategoriaCommand(personaId, categoriaId);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Categoría asignada exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Recurso no encontrado",
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
    /// Remueve una categoría de una persona
    /// </summary>
    [HttpDelete("{personaId}/categorias/{categoriaId}")]
    public async Task<ActionResult<ApiResponse>> RemoverCategoria(
        int personaId,
        int categoriaId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new RemoverCategoriaCommand(personaId, categoriaId);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Categoría removida exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Relación no encontrada",
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
}