using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Maestros.Cargo.Commands.CreateCargo;
using Miski.Application.Features.Maestros.Cargo.Commands.UpdateCargo;
using Miski.Application.Features.Maestros.Cargo.Commands.DeleteCargo;
using Miski.Application.Features.Maestros.Cargo.Commands.AsignarCargo;
using Miski.Application.Features.Maestros.Cargo.Commands.RevocarCargo;
using Miski.Application.Features.Maestros.Cargo.Queries.GetCargos;
using Miski.Application.Features.Maestros.Cargo.Queries.GetCargoById;
using Miski.Application.Features.Maestros.Cargo.Queries.GetCargosByPersona;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Api.Controllers.Maestros;

[ApiController]
[Route("api/maestros/cargos")]
[Tags("Maestros")]
[Authorize]
public class CargosController : ControllerBase
{
    private readonly IMediator _mediator;

    public CargosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los cargos
    /// </summary>
    /// <param name="estado">Filtrar por estado (opcional)</param>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CargoDto>>>> GetCargos(
        [FromQuery] string? estado = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetCargosQuery(estado);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<IEnumerable<CargoDto>>.SuccessResult(
                result,
                "Cargos obtenidos exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<CargoDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene un cargo por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CargoDto>>> GetCargoById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetCargoByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<CargoDto>.SuccessResult(
                result,
                "Cargo obtenido exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<CargoDto>.ErrorResult(
                "Cargo no encontrado",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CargoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene los cargos asignados a una persona
    /// </summary>
    /// <param name="idPersona">ID de la persona</param>
    /// <param name="soloActuales">Filtrar solo cargos actuales (opcional)</param>
    [HttpGet("persona/{idPersona}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<PersonaCargoDto>>>> GetCargosByPersona(
        int idPersona,
        [FromQuery] bool? soloActuales = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetCargosByPersonaQuery(idPersona, soloActuales);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<IEnumerable<PersonaCargoDto>>.SuccessResult(
                result,
                "Cargos de la persona obtenidos exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<IEnumerable<PersonaCargoDto>>.ErrorResult(
                "Persona no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<PersonaCargoDto>>.ErrorResult(
                "Error al obtener cargos de la persona",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea un nuevo cargo
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CargoDto>>> CreateCargo(
        [FromBody] CreateCargoDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateCargoCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetCargoById),
                new { id = result.IdCargo },
                ApiResponse<CargoDto>.SuccessResult(
                    result,
                    "Cargo creado exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<CargoDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CargoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Asigna un cargo a una persona
    /// </summary>
    /// <remarks>
    /// Al asignar un nuevo cargo, se desactivan automáticamente los cargos actuales de la persona.
    /// </remarks>
    [HttpPost("asignar")]
    public async Task<ActionResult<ApiResponse<PersonaCargoDto>>> AsignarCargo(
        [FromBody] AsignarCargoDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new AsignarCargoCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<PersonaCargoDto>.SuccessResult(
                result,
                "Cargo asignado exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<PersonaCargoDto>.ErrorResult(
                "Entidad no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<PersonaCargoDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<PersonaCargoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Revoca un cargo asignado a una persona
    /// </summary>
    /// <remarks>
    /// Marca el cargo como no actual y establece una fecha de fin.
    /// </remarks>
    [HttpPost("revocar")]
    public async Task<ActionResult<ApiResponse<PersonaCargoDto>>> RevocarCargo(
        [FromBody] RevocarCargoDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new RevocarCargoCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<PersonaCargoDto>.SuccessResult(
                result,
                "Cargo revocado exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<PersonaCargoDto>.ErrorResult(
                "Asignación de cargo no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<PersonaCargoDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<PersonaCargoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza un cargo
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CargoDto>>> UpdateCargo(
        int id,
        [FromBody] UpdateCargoDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdCargo)
            {
                return BadRequest(ApiResponse<CargoDto>.ErrorResult(
                    "ID inválido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petición"
                ));
            }

            var command = new UpdateCargoCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<CargoDto>.SuccessResult(
                result,
                "Cargo actualizado exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<CargoDto>.ErrorResult(
                "Cargo no encontrado",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<CargoDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CargoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Elimina (inactiva) un cargo
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteCargo(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteCargoCommand(id);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Cargo eliminado exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Cargo no encontrado",
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
