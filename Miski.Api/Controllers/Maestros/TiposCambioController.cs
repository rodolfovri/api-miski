using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Maestros.TipoCambio.Commands.CreateTipoCambio;
using Miski.Application.Features.Maestros.TipoCambio.Commands.UpdateTipoCambio;
using Miski.Application.Features.Maestros.TipoCambio.Commands.DeleteTipoCambio;
using Miski.Application.Features.Maestros.TipoCambio.Queries.GetTiposCambio;
using Miski.Application.Features.Maestros.TipoCambio.Queries.GetTipoCambioById;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Api.Controllers.Maestros;

[ApiController]
[Route("api/maestros/tipos-cambio")]
[Tags("Maestros")]
[Authorize]
public class TiposCambioController : ControllerBase
{
    private readonly IMediator _mediator;

    public TiposCambioController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los tipos de cambio con filtros opcionales
    /// </summary>
    /// <remarks>
    /// Permite filtrar por:
    /// - idMoneda: Filtrar por moneda específica
    /// - fechaDesde: Fecha de inicio del rango
    /// - fechaHasta: Fecha fin del rango
    /// 
    /// Los resultados se ordenan por fecha de registro descendente (más reciente primero)
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<TipoCambioDto>>>> GetTiposCambio(
        [FromQuery] int? idMoneda = null,
        [FromQuery] DateTime? fechaDesde = null,
        [FromQuery] DateTime? fechaHasta = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetTiposCambioQuery(idMoneda, fechaDesde, fechaHasta);
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<TipoCambioDto>>.SuccessResult(
                result, 
                "Tipos de cambio obtenidos exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<TipoCambioDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene un tipo de cambio por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<TipoCambioDto>>> GetTipoCambioById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetTipoCambioByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<TipoCambioDto>.SuccessResult(
                result,
                "Tipo de cambio obtenido exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<TipoCambioDto>.ErrorResult(
                "Tipo de cambio no encontrado",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TipoCambioDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea un nuevo tipo de cambio
    /// </summary>
    /// <remarks>
    /// Registra el tipo de cambio de una moneda en una fecha específica.
    /// 
    /// Validaciones:
    /// - La moneda debe existir
    /// - El usuario debe existir
    /// - El valor de compra debe ser mayor a 0 y menor a 1000
    /// - El valor de venta debe ser mayor a 0 y menor a 1000
    /// - El valor de venta debe ser mayor o igual al valor de compra
    /// 
    /// Ejemplo:
    /// {
    ///   "idMoneda": 1,
    ///   "idUsuario": 5,
    ///   "valorCompra": 3.70,
    ///   "valorVenta": 3.75
    /// }
    /// </remarks>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<TipoCambioDto>>> CreateTipoCambio(
        [FromBody] CreateTipoCambioDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateTipoCambioCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetTipoCambioById), 
                new { id = result.IdTipoCambio }, 
                ApiResponse<TipoCambioDto>.SuccessResult(
                    result, 
                    "Tipo de cambio creado exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<TipoCambioDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<TipoCambioDto>.ErrorResult(
                "Entidad relacionada no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TipoCambioDto>.ErrorResult(
                "Error interno del servidor", 
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza un tipo de cambio
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<TipoCambioDto>>> UpdateTipoCambio(
        int id,
        [FromBody] UpdateTipoCambioDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdTipoCambio)
            {
                return BadRequest(ApiResponse<TipoCambioDto>.ErrorResult(
                    "ID inválido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petición"
                ));
            }

            var command = new UpdateTipoCambioCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<TipoCambioDto>.SuccessResult(
                result,
                "Tipo de cambio actualizado exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<TipoCambioDto>.ErrorResult(
                "Tipo de cambio no encontrado",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<TipoCambioDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TipoCambioDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Elimina un tipo de cambio
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteTipoCambio(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteTipoCambioCommand(id);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Tipo de cambio eliminado exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Tipo de cambio no encontrado",
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
