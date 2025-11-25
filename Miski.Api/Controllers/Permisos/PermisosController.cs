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
    /// Obtiene la estructura completa de módulos con jerarquía y acciones disponibles
    /// </summary>
    /// <remarks>
    /// **Estructura de Acciones:**
    /// - Cada SubMódulo y SubMóduloDetalle tiene definidas las acciones disponibles
    /// - SubMódulo con TieneDetalles = false: Muestra acciones disponibles directamente
    /// - SubMódulo con TieneDetalles = true: Muestra acciones en cada SubMóduloDetalle
    /// - Las acciones disponibles se definen en las tablas SubModuloAccion y SubModuloDetalleAccion
    /// </remarks>
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
    /// **Catálogo Global de Acciones:**
    /// 
    /// Las acciones son permisos granulares que se pueden asignar a pantallas específicas.
    /// 
    /// **Ejemplos de acciones comunes:**
    /// - VER (ver/listar datos)
    /// - CREAR (crear nuevos registros)
    /// - EDITAR (modificar registros existentes)
    /// - ELIMINAR (eliminar registros)
    /// - APROBAR (aprobar transacciones)
    /// - RECHAZAR (rechazar solicitudes)
    /// - EXPORTAR (exportar datos)
    /// - IMPRIMIR (imprimir documentos)
    /// 
    /// **Flujo de uso:**
    /// 1. Crear acciones en el catálogo global (esta tabla Accion)
    /// 2. Asignar qué acciones están disponibles para cada pantalla (SubModuloAccion o SubModuloDetalleAccion)
    /// 3. Habilitar/deshabilitar acciones específicas por rol (PermisoRolAccion)
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
    /// Crea una nueva acción en el catálogo global del sistema
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

    /// <summary>
    /// Asigna una acción disponible a un SubMódulo (pantalla sin detalles)
    /// </summary>
    /// <remarks>
    /// **Propósito:**
    /// Define qué acciones están disponibles para ejecutar en un SubMódulo específico.
    /// 
    /// **Validación:**
    /// - El SubMódulo debe tener TieneDetalles = false
    /// - Si TieneDetalles = true, use el endpoint para SubMóduloDetalle
    /// 
    /// **Ejemplo:**
    /// Para la pantalla "Gestión de Productos" (SubMódulo sin detalles):
    /// ```json
    /// {
    ///   "idSubModulo": 5,
    ///   "idAccion": 1,  // VER
    ///   "habilitado": true
    /// }
    /// ```
    /// </remarks>
    [HttpPost("submodulo-acciones")]
    public async Task<ActionResult<ApiResponse<SubModuloAccionDto>>> AsignarAccionSubModulo(
        [FromQuery] int idSubModulo,
        [FromQuery] int idAccion,
        [FromQuery] bool habilitado = true,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new Application.Features.Permisos.Commands.AsignarAccionSubModulo.AsignarAccionSubModuloCommand(
                idSubModulo, idAccion, habilitado);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<SubModuloAccionDto>.SuccessResult(
                result,
                "Acción asignada al SubMódulo exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<SubModuloAccionDto>.ErrorResult(
                "Recurso no encontrado",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<SubModuloAccionDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<SubModuloAccionDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Asigna una acción disponible a un SubMóduloDetalle (pantalla dentro de un SubMódulo)
    /// </summary>
    /// <remarks>
    /// **Propósito:**
    /// Define qué acciones están disponibles para ejecutar en un SubMóduloDetalle específico.
    /// 
    /// **Ejemplo:**
    /// Para la pantalla "Unidad de Medida" dentro de "Tablas Maestras":
    /// ```json
    /// {
    ///   "idSubModuloDetalle": 12,
    ///   "idAccion": 2,  // CREAR
    ///   "habilitado": true
    /// }
    /// ```
    /// </remarks>
    [HttpPost("submodulo-detalle-acciones")]
    public async Task<ActionResult<ApiResponse<SubModuloDetalleAccionDto>>> AsignarAccionSubModuloDetalle(
        [FromQuery] int idSubModuloDetalle,
        [FromQuery] int idAccion,
        [FromQuery] bool habilitado = true,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new Application.Features.Permisos.Commands.AsignarAccionSubModuloDetalle.AsignarAccionSubModuloDetalleCommand(
                idSubModuloDetalle, idAccion, habilitado);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<SubModuloDetalleAccionDto>.SuccessResult(
                result,
                "Acción asignada al SubMóduloDetalle exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<SubModuloDetalleAccionDto>.ErrorResult(
                "Recurso no encontrado",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<SubModuloDetalleAccionDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<SubModuloDetalleAccionDto>.ErrorResult(
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
    /// **Acciones Disponibles vs Habilitadas:**
    /// - **Acciones Disponibles**: Definidas en SubModuloAccion o SubModuloDetalleAccion
    ///   (qué acciones están configuradas para cada pantalla)
    /// - **Acciones Habilitadas**: Definidas en PermisoRolAccion
    ///   (qué acciones puede ejecutar este rol en esta pantalla)
    /// 
    /// **Respuesta:**
    /// - Solo se muestran las acciones disponibles para cada pantalla
    /// - El campo "Habilitado" indica si el rol tiene permiso para ejecutar esa acción
    /// 
    /// **Ejemplo de respuesta:**
    /// ```json
    /// {
    ///   "subModulo": {
    ///     "nombre": "Gestión de Productos",
    ///     "tieneDetalles": false,
    ///     "acciones": [
    ///       { "codigo": "VER", "habilitado": true },
    ///       { "codigo": "CREAR", "habilitado": true },
    ///       { "codigo": "EDITAR", "habilitado": false }
    ///     ]
    ///   }
    /// }
    /// ```
    /// En este caso, la pantalla tiene 3 acciones disponibles, pero el rol solo tiene habilitadas VER y CREAR.
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
    /// **Validaciones de Acciones:**
    /// - Solo se pueden asignar acciones que estén definidas como disponibles en SubModuloAccion o SubModuloDetalleAccion
    /// - No se pueden asignar acciones a nivel de Módulo (solo a SubMódulo o Detalle)
    /// - Si el SubMódulo tiene detalles (TieneDetalles = true), las acciones deben asignarse a los detalles
    /// 
    /// **Flujo recomendado:**
    /// 1. Primero, define qué acciones están disponibles para cada pantalla (SubModuloAccion/SubModuloDetalleAccion)
    /// 2. Luego, habilita las acciones que cada rol puede ejecutar (este endpoint)
    /// 
    /// **Ejemplo para SubMódulo sin detalles:**
    /// ```json
    /// {
    ///   "idRol": 1,
    ///   "idModulo": 2,
    ///   "idSubModulo": 3,
    ///   "tieneAcceso": true,
    ///   "idAcciones": [1, 2, 3]  // VER, CREAR, EDITAR (deben estar en SubModuloAccion)
    /// }
    /// ```
    /// 
    /// **Ejemplo para SubMóduloDetalle:**
    /// ```json
    /// {
    ///   "idRol": 1,
    ///   "idModulo": 2,
    ///   "idSubModulo": 3,
    ///   "idSubModuloDetalle": 5,
    ///   "tieneAcceso": true,
    ///   "idAcciones": [1, 2]  // VER, CREAR (deben estar en SubModuloDetalleAccion)
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
    /// Cada permiso debe cumplir las mismas validaciones que el endpoint individual.
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