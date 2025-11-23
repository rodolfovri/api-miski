using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Tracking.Commands.RegistrarUbicacion;
using Miski.Application.Features.Tracking.Commands.RegistrarUbicacionDispositivo;
using Miski.Application.Features.Tracking.Queries.GetHistorialUbicaciones;
using Miski.Application.Features.Tracking.Queries.GetUbicacionActual;
using Miski.Application.Features.Tracking.Queries.GetUbicacionesActivas;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Tracking;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Miski.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Gestión de tracking y ubicaciones en tiempo real")]
public class TrackingController : ControllerBase
{
    private readonly IMediator _mediator;

    public TrackingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Registrar ubicación desde dispositivo sin autenticación (background tracking)
    /// </summary>
    /// <remarks>
    /// **IMPORTANTE:** Este endpoint NO requiere autenticación JWT.
    /// 
    /// Se usa para tracking en background cuando:
    /// - La app está cerrada
    /// - El usuario no tiene sesión activa
    /// - El tracking continúa en segundo plano
    /// 
    /// **Validaciones de seguridad:**
    /// - El DeviceId debe estar registrado previamente (durante el login)
    /// - El DeviceId debe estar activo
    /// - El IdPersona debe coincidir con el dispositivo registrado
    /// </remarks>
    [HttpPost("registrar-ubicacion-dispositivo")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Registrar ubicación por dispositivo (sin JWT)")]
    [SwaggerResponse(200, "Ubicación registrada exitosamente")]
    [SwaggerResponse(400, "Datos inválidos o dispositivo no autorizado")]
    public async Task<IActionResult> RegistrarUbicacionDispositivo(
        [FromBody] UbicacionDispositivoDto dto)
    {
        var command = new RegistrarUbicacionDispositivoCommand { Data = dto };
        await _mediator.Send(command);

        return Ok(ApiResponse.SuccessResult("Ubicación registrada exitosamente"));
    }

    /// <summary>
    /// Registrar ubicación con usuario autenticado
    /// </summary>
    /// <remarks>
    /// **Requiere autenticación JWT.**
    /// 
    /// Se usa cuando el usuario está activo en la app.
    /// El IdPersona se obtiene automáticamente del token JWT.
    /// </remarks>
    [HttpPost("registrar-ubicacion")]
    [Authorize]
    [SwaggerOperation(Summary = "Registrar ubicación con usuario autenticado")]
    [SwaggerResponse(200, "Ubicación registrada exitosamente")]
    [SwaggerResponse(401, "No autorizado")]
    [SwaggerResponse(400, "Datos inválidos")]
    public async Task<IActionResult> RegistrarUbicacion(
        [FromBody] RegistrarUbicacionDto dto)
    {
        // Obtener IdPersona del JWT
        var personaIdClaim = User.FindFirst("PersonaId")?.Value;
        if (string.IsNullOrEmpty(personaIdClaim) || !int.TryParse(personaIdClaim, out var idPersona))
        {
            return Unauthorized(ApiResponse.ErrorResult("Token inválido o IdPersona no encontrado"));
        }

        var command = new RegistrarUbicacionCommand
        {
            IdPersona = idPersona,
            Data = dto
        };

        await _mediator.Send(command);

        return Ok(ApiResponse.SuccessResult("Ubicación registrada exitosamente"));
    }

    /// <summary>
    /// Obtener la ubicación actual de una persona
    /// </summary>
    [HttpGet("ubicacion-actual/{idPersona}")]
    [Authorize]
    [SwaggerOperation(Summary = "Obtener ubicación actual de una persona")]
    [SwaggerResponse(200, "Ubicación actual obtenida", typeof(ApiResponse))]
    [SwaggerResponse(404, "Persona no tiene ubicación registrada")]
    public async Task<IActionResult> GetUbicacionActual(int idPersona)
    {
        var query = new GetUbicacionActualQuery { IdPersona = idPersona };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(ApiResponse.ErrorResult("No se encontró ubicación actual para esta persona"));
        }

        return Ok(ApiResponse<TrackingResponseDto>.SuccessResult(result));
    }

    /// <summary>
    /// Obtener el historial de ubicaciones de una persona
    /// </summary>
    [HttpGet("historial/{idPersona}")]
    [Authorize]
    [SwaggerOperation(Summary = "Obtener historial de ubicaciones")]
    [SwaggerResponse(200, "Historial obtenido", typeof(ApiResponse))]
    public async Task<IActionResult> GetHistorialUbicaciones(
        int idPersona,
        [FromQuery] DateTime? fechaInicio,
        [FromQuery] DateTime? fechaFin,
        [FromQuery] int? limite = 100)
    {
        var query = new GetHistorialUbicacionesQuery
        {
            IdPersona = idPersona,
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            Limite = limite
        };

        var result = await _mediator.Send(query);

        return Ok(ApiResponse<List<TrackingResponseDto>>.SuccessResult(
            result,
            $"{result.Count} ubicaciones encontradas"));
    }

    /// <summary>
    /// Obtener todas las ubicaciones activas (mapa en tiempo real)
    /// </summary>
    /// <remarks>
    /// Retorna la última ubicación conocida de todas las personas con tracking activo.
    /// Útil para mostrar en un mapa todas las posiciones actuales.
    /// </remarks>
    [HttpGet("ubicaciones-activas")]
    [Authorize]
    [SwaggerOperation(Summary = "Obtener todas las ubicaciones activas")]
    [SwaggerResponse(200, "Ubicaciones activas obtenidas", typeof(ApiResponse))]
    public async Task<IActionResult> GetUbicacionesActivas()
    {
        var query = new GetUbicacionesActivasQuery();
        var result = await _mediator.Send(query);

        return Ok(ApiResponse<List<TrackingResponseDto>>.SuccessResult(
            result,
            $"{result.Count} ubicaciones activas"));
    }

    /// <summary>
    /// Obtener mi ubicación actual (del usuario autenticado)
    /// </summary>
    [HttpGet("mi-ubicacion")]
    [Authorize]
    [SwaggerOperation(Summary = "Obtener mi ubicación actual")]
    [SwaggerResponse(200, "Ubicación obtenida", typeof(ApiResponse))]
    [SwaggerResponse(404, "No tienes ubicación registrada")]
    public async Task<IActionResult> GetMiUbicacion()
    {
        var personaIdClaim = User.FindFirst("PersonaId")?.Value;
        if (string.IsNullOrEmpty(personaIdClaim) || !int.TryParse(personaIdClaim, out var idPersona))
        {
            return Unauthorized(ApiResponse.ErrorResult("Token inválido"));
        }

        var query = new GetUbicacionActualQuery { IdPersona = idPersona };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(ApiResponse.ErrorResult("No tienes ubicación registrada"));
        }

        return Ok(ApiResponse<TrackingResponseDto>.SuccessResult(result));
    }
}
