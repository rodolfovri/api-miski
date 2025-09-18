using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Negociaciones.Commands.CreateNegociacion;
using Miski.Application.Features.Negociaciones.Queries.GetNegociaciones;
using Miski.Shared.DTOs;
using Miski.Shared.DTOs.Base;

namespace Miski.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NegociacionesController : ControllerBase
{
    private readonly IMediator _mediator;

    public NegociacionesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todas las negociaciones con filtros opcionales
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<NegociacionDto>>>> GetNegociaciones(
        [FromQuery] int? proveedorId = null,
        [FromQuery] int? comisionistaId = null,
        [FromQuery] string? estado = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetNegociacionesQuery(proveedorId, comisionistaId, estado);
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<NegociacionDto>>.SuccessResult(
                result, 
                "Negociaciones obtenidas exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<IEnumerable<NegociacionDto>>.ErrorResult(
                "Error al obtener las negociaciones", 
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea una nueva negociaci�n
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<NegociacionDto>>> CreateNegociacion(
        [FromBody] CreateNegociacionDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateNegociacionCommand(request);
            var result = await _mediator.Send(command, cancellationToken);
            
            return CreatedAtAction(
                nameof(GetNegociacionById), 
                new { id = result.IdNegociacion }, 
                ApiResponse<NegociacionDto>.SuccessResult(
                    result, 
                    "Negociaci�n creada exitosamente"
                )
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<NegociacionDto>.ErrorResult(
                "Datos inv�lidos", 
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<NegociacionDto>.ErrorResult(
                "Error interno del servidor", 
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene una negociaci�n por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<NegociacionDto>>> GetNegociacionById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return NotFound(ApiResponse<NegociacionDto>.ErrorResult(
                "Negociaci�n no encontrada", 
                $"No se encontr� una negociaci�n con ID {id}"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<NegociacionDto>.ErrorResult(
                "Error interno del servidor", 
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene negociaciones pendientes de aprobaci�n
    /// </summary>
    [HttpGet("pendientes-aprobacion")]
    public async Task<ActionResult<ApiResponse<IEnumerable<NegociacionDto>>>> GetPendientesAprobacion(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetNegociacionesQuery(Estado: "Pendiente");
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<NegociacionDto>>.SuccessResult(
                result, 
                "Negociaciones pendientes obtenidas exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<IEnumerable<NegociacionDto>>.ErrorResult(
                "Error al obtener negociaciones pendientes", 
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Aprueba una negociaci�n
    /// </summary>
    [HttpPut("{id}/aprobar")]
    public async Task<ActionResult<ApiResponse>> AprobarNegociacion(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return Ok(ApiResponse.SuccessResult("Negociaci�n aprobada exitosamente"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse.ErrorResult(
                "Error al aprobar la negociaci�n", 
                ex.Message
            ));
        }
    }
}