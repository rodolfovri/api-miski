using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Maestros.Rol.Commands.CreateRol;
using Miski.Application.Features.Maestros.Rol.Commands.UpdateRol;
using Miski.Application.Features.Maestros.Rol.Commands.DeleteRol;
using Miski.Application.Features.Maestros.Rol.Queries.GetRoles;
using Miski.Application.Features.Maestros.Rol.Queries.GetRolById;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Api.Controllers.Maestros;

[ApiController]
[Route("api/maestros/roles")]
[Tags("Maestros")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los roles
    /// </summary>
    /// <param name="tipoPlataforma">Filtrar por tipo de plataforma (opcional)</param>
    /// <param name="estado">Filtrar por estado (opcional): ACTIVO o INACTIVO</param>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<RolMaestroDto>>>> GetRoles(
        [FromQuery] string? tipoPlataforma = null,
        [FromQuery] string? estado = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetRolesQuery(tipoPlataforma, estado);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<IEnumerable<RolMaestroDto>>.SuccessResult(
                result,
                "Roles obtenidos exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<RolMaestroDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene un rol por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<RolMaestroDto>>> GetRolById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetRolByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<RolMaestroDto>.SuccessResult(
                result,
                "Rol obtenido exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<RolMaestroDto>.ErrorResult(
                "Rol no encontrado",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<RolMaestroDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea un nuevo rol
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<RolMaestroDto>>> CreateRol(
        [FromBody] CreateRolDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateRolCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetRolById),
                new { id = result.IdRol },
                ApiResponse<RolMaestroDto>.SuccessResult(
                    result,
                    "Rol creado exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<RolMaestroDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<RolMaestroDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza un rol
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<RolMaestroDto>>> UpdateRol(
        int id,
        [FromBody] UpdateRolDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdRol)
            {
                return BadRequest(ApiResponse<RolMaestroDto>.ErrorResult(
                    "ID inválido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petición"
                ));
            }

            var command = new UpdateRolCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<RolMaestroDto>.SuccessResult(
                result,
                "Rol actualizado exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<RolMaestroDto>.ErrorResult(
                "Rol no encontrado",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<RolMaestroDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<RolMaestroDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Elimina un rol
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteRol(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteRolCommand(id);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Rol eliminado exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Rol no encontrado",
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
