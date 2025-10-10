using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Permisos.Commands.AsignarPermiso;
using Miski.Application.Features.Permisos.Queries.GetPermisosPorRol;
using Miski.Application.Features.Permisos.Queries.GetModulos;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Permisos;

namespace Miski.Api.Controllers.Permisos;

[ApiController]
[Route("api/permisos")]
[Tags("Permisos")]
[Authorize]
public class PermisosController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermisosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene la estructura completa de módulos con jerarquía
    /// </summary>
    [HttpGet("modulos")]
    public async Task<ActionResult<ApiResponse<List<ModuloDto>>>> GetModulos(
        [FromQuery] string? tipoPlataforma = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetModulosQuery(tipoPlataforma);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<List<ModuloDto>>.SuccessResult(
                result,
                "Módulos obtenidos exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<ModuloDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene todos los permisos de un rol con jerarquía completa
    /// </summary>
    /// <remarks>
    /// Si un rol tiene acceso a un Módulo, automáticamente accede a todos sus subniveles.
    /// Si tiene acceso solo a un SubMódulo o SubMóduloDetalle, el acceso es limitado a ese nivel.
    /// </remarks>
    [HttpGet("rol/{idRol}")]
    public async Task<ActionResult<ApiResponse<PermisoRolConJerarquiaDto>>> GetPermisosPorRol(
        int idRol,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetPermisosPorRolQuery(idRol);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<PermisoRolConJerarquiaDto>.SuccessResult(
                result,
                "Permisos del rol obtenidos exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<PermisoRolConJerarquiaDto>.ErrorResult(
                "Rol no encontrado",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<PermisoRolConJerarquiaDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Asigna o actualiza un permiso específico a un rol
    /// </summary>
    /// <remarks>
    /// Puede asignar permisos a nivel de:
    /// - Módulo completo (acceso a todo)
    /// - SubMódulo específico
    /// - SubMóduloDetalle específico
    /// </remarks>
    [HttpPost("asignar")]
    public async Task<ActionResult<ApiResponse<PermisoRolDto>>> AsignarPermiso(
        [FromBody] AsignarPermisoDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new AsignarPermisoCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<PermisoRolDto>.SuccessResult(
                result,
                "Permiso asignado exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<PermisoRolDto>.ErrorResult(
                "Recurso no encontrado",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<PermisoRolDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<PermisoRolDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Asigna múltiples permisos a un rol de una sola vez
    /// </summary>
    [HttpPost("asignar-multiples")]
    public async Task<ActionResult<ApiResponse<List<PermisoRolDto>>>> AsignarPermisosMultiples(
        [FromBody] AsignarPermisosMultiplesDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var resultados = new List<PermisoRolDto>();

            foreach (var permiso in request.Permisos)
            {
                var command = new AsignarPermisoCommand(new AsignarPermisoDto
                {
                    IdRol = request.IdRol,
                    IdModulo = permiso.IdModulo,
                    IdSubModulo = permiso.IdSubModulo,
                    IdSubModuloDetalle = permiso.IdSubModuloDetalle,
                    TieneAcceso = permiso.TieneAcceso
                });

                var result = await _mediator.Send(command, cancellationToken);
                resultados.Add(result);
            }

            return Ok(ApiResponse<List<PermisoRolDto>>.SuccessResult(
                resultados,
                $"{resultados.Count} permisos asignados exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<List<PermisoRolDto>>.ErrorResult(
                "Recurso no encontrado",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<PermisoRolDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Revoca un permiso específico de un rol
    /// </summary>
    [HttpDelete("revocar")]
    public async Task<ActionResult<ApiResponse>> RevocarPermiso(
        [FromQuery] int idRol,
        [FromQuery] int? idModulo = null,
        [FromQuery] int? idSubModulo = null,
        [FromQuery] int? idSubModuloDetalle = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Asignar con TieneAcceso = false equivale a revocar
            var command = new AsignarPermisoCommand(new AsignarPermisoDto
            {
                IdRol = idRol,
                IdModulo = idModulo,
                IdSubModulo = idSubModulo,
                IdSubModuloDetalle = idSubModuloDetalle,
                TieneAcceso = false
            });

            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Permiso revocado exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Recurso no encontrado",
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