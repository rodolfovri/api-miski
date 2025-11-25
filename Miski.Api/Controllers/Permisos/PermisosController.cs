using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Permisos.Commands.AsignarPermiso;
using Miski.Application.Features.Permisos.Commands.CreateAccion;
using Miski.Application.Features.Permisos.Queries.GetPermisosPorRol;
using Miski.Application.Features.Permisos.Queries.GetModulos;
using Miski.Application.Features.Permisos.Queries.GetAcciones;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Permisos;
using Swashbuckle.AspNetCore.Annotations;

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

    // ==================== MÓDULOS ====================

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

    // ==================== ACCIONES ====================

    /// <summary>
    /// Obtiene todas las acciones disponibles del sistema
    /// </summary>
    /// <remarks>
    /// Las acciones son los permisos granulares (Ver, Crear, Editar, Eliminar, Aprobar, etc.)
    /// que se pueden asignar a nivel de SubMódulo o SubMóduloDetalle.
    /// </remarks>
    [HttpGet("acciones")]
    public async Task<ActionResult<ApiResponse<List<AccionDto>>>> GetAcciones(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetAccionesQuery();
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<List<AccionDto>>.SuccessResult(
                result,
                $"{result.Count} acciones encontradas"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<AccionDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea una nueva acción en el sistema
    /// </summary>
    /// <remarks>
    /// Ejemplos de acciones:
    /// - VER (ver datos)
    /// - CREAR (crear nuevos registros)
    /// - EDITAR (modificar registros)
    /// - ELIMINAR (eliminar registros)
    /// - APROBAR (aprobar transacciones)
    /// - RECHAZAR (rechazar solicitudes)
    /// </remarks>
    [HttpPost("acciones")]
    public async Task<ActionResult<ApiResponse<AccionDto>>> CreateAccion(
        [FromBody] CreateAccionDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateAccionCommand { Data = request };
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<AccionDto>.SuccessResult(
                result,
                "Acción creada exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<AccionDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    // ==================== PERMISOS DE ROL ====================

    /// <summary>
    /// Obtiene todos los permisos de un rol con jerarquía completa y acciones
    /// </summary>
    /// <remarks>
    /// **Lógica de Herencia:**
    /// - Si un rol tiene acceso a un Módulo ? Accede a todos sus SubMódulos y Detalles
    /// - Si tiene acceso a un SubMódulo ? Accede a todos sus Detalles
    /// - Si tiene acceso a un Detalle ? Acceso limitado solo a ese detalle
    /// 
    /// **Acciones:**
    /// - Se muestran las acciones disponibles según el nivel (SubMódulo o Detalle)
    /// - Indica si cada acción está habilitada para el rol
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
    /// Asigna o actualiza un permiso específico a un rol con acciones
    /// </summary>
    /// <remarks>
    /// **Niveles de asignación:**
    /// - **Módulo completo**: Enviar solo `idModulo`
    /// - **SubMódulo específico**: Enviar `idModulo` + `idSubModulo`
    /// - **Detalle específico**: Enviar los 3 IDs
    /// 
    /// **Acciones:**
    /// - `idAcciones`: Array de IDs de acciones a habilitar
    /// - Si `TieneDetalles = false`: Las acciones se asignan al SubMódulo
    /// - Si `TieneDetalles = true`: Las acciones se asignan al Detalle
    /// 
    /// **Ejemplo:**
    /// ```json
    /// {
    ///   "idRol": 1,
    ///   "idModulo": 2,
    ///   "idSubModulo": 3,
    ///   "tieneAcceso": true,
    ///   "idAcciones": [1, 2, 3]  // VER, CREAR, EDITAR
    /// }
    /// ```
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
    /// <remarks>
    /// Útil para configurar todos los permisos de un rol en una sola operación.
    /// </remarks>
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
                    TieneAcceso = permiso.TieneAcceso,
                    IdAcciones = permiso.IdAcciones
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