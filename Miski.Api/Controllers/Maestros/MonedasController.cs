using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Maestros.Moneda.Commands.CreateMoneda;
using Miski.Application.Features.Maestros.Moneda.Commands.UpdateMoneda;
using Miski.Application.Features.Maestros.Moneda.Commands.DeleteMoneda;
using Miski.Application.Features.Maestros.Moneda.Queries.GetMonedas;
using Miski.Application.Features.Maestros.Moneda.Queries.GetMonedaById;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Api.Controllers.Maestros;

[ApiController]
[Route("api/maestros/monedas")]
[Tags("Maestros")]
[Authorize]
public class MonedasController : ControllerBase
{
    private readonly IMediator _mediator;

    public MonedasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todas las monedas
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<MonedaDto>>>> GetMonedas(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetMonedasQuery();
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<MonedaDto>>.SuccessResult(
                result, 
                "Monedas obtenidas exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<MonedaDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene una moneda por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<MonedaDto>>> GetMonedaById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetMonedaByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<MonedaDto>.SuccessResult(
                result,
                "Moneda obtenida exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<MonedaDto>.ErrorResult(
                "Moneda no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<MonedaDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea una nueva moneda
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<MonedaDto>>> CreateMoneda(
        [FromBody] CreateMonedaDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateMonedaCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetMonedaById), 
                new { id = result.IdMoneda }, 
                ApiResponse<MonedaDto>.SuccessResult(
                    result, 
                    "Moneda creada exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<MonedaDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<MonedaDto>.ErrorResult(
                "Error interno del servidor", 
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza una moneda
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<MonedaDto>>> UpdateMoneda(
        int id,
        [FromBody] UpdateMonedaDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdMoneda)
            {
                return BadRequest(ApiResponse<MonedaDto>.ErrorResult(
                    "ID inválido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petición"
                ));
            }

            var command = new UpdateMonedaCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<MonedaDto>.SuccessResult(
                result,
                "Moneda actualizada exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<MonedaDto>.ErrorResult(
                "Moneda no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<MonedaDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<MonedaDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Elimina una moneda
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteMoneda(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteMonedaCommand(id);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Moneda eliminada exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Moneda no encontrada",
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
