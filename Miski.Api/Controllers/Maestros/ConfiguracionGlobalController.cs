using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Maestros.ConfiguracionGlobal.Commands.CreateConfiguracion;
using Miski.Application.Features.Maestros.ConfiguracionGlobal.Commands.DeleteConfiguracion;
using Miski.Application.Features.Maestros.ConfiguracionGlobal.Commands.UpdateConfiguracion;
using Miski.Application.Features.Maestros.ConfiguracionGlobal.Queries.GetConfiguracionByClave;
using Miski.Application.Features.Maestros.ConfiguracionGlobal.Queries.GetConfiguracionById;
using Miski.Application.Features.Maestros.ConfiguracionGlobal.Queries.GetConfiguraciones;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Api.Controllers.Maestros;

[ApiController]
[Route("api/maestros/configuraciones")]
[Tags("Maestros")]
[Authorize]
public class ConfiguracionGlobalController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConfiguracionGlobalController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todas las configuraciones globales
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ConfiguracionGlobalDto>>>> GetConfiguraciones(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetConfiguracionesQuery();
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<IEnumerable<ConfiguracionGlobalDto>>.SuccessResult(
                result,
                "Configuraciones obtenidas exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<ConfiguracionGlobalDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene una configuración por su ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ConfiguracionGlobalDto>>> GetConfiguracionById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetConfiguracionByIdQuery { IdConfiguracionGlobal = id };
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<ConfiguracionGlobalDto>.SuccessResult(
                result,
                "Configuración obtenida exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<ConfiguracionGlobalDto>.ErrorResult(
                "Configuración no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<ConfiguracionGlobalDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene una configuración por su clave
    /// </summary>
    [HttpGet("clave/{clave}")]
    public async Task<ActionResult<ApiResponse<ConfiguracionGlobalDto>>> GetConfiguracionByClave(
        string clave,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetConfiguracionByClaveQuery { Clave = clave };
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<ConfiguracionGlobalDto>.SuccessResult(
                result,
                "Configuración obtenida exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<ConfiguracionGlobalDto>.ErrorResult(
                "Configuración no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<ConfiguracionGlobalDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea una nueva configuración global
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ConfiguracionGlobalDto>>> CreateConfiguracion(
        [FromBody] CreateConfiguracionGlobalDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateConfiguracionCommand { ConfiguracionData = request };
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetConfiguracionById),
                new { id = result.IdConfiguracionGlobal },
                ApiResponse<ConfiguracionGlobalDto>.SuccessResult(
                    result,
                    "Configuración creada exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<ConfiguracionGlobalDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<ConfiguracionGlobalDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza una configuración existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ConfiguracionGlobalDto>>> UpdateConfiguracion(
        int id,
        [FromBody] UpdateConfiguracionGlobalDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdConfiguracionGlobal)
            {
                return BadRequest(ApiResponse<ConfiguracionGlobalDto>.ErrorResult(
                    "ID inválido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petición"
                ));
            }

            var command = new UpdateConfiguracionCommand { ConfiguracionData = request };
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<ConfiguracionGlobalDto>.SuccessResult(
                result,
                "Configuración actualizada exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<ConfiguracionGlobalDto>.ErrorResult(
                "Configuración no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<ConfiguracionGlobalDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<ConfiguracionGlobalDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Elimina una configuración
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteConfiguracion(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteConfiguracionCommand { IdConfiguracionGlobal = id };
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Configuración eliminada exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Configuración no encontrada",
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
