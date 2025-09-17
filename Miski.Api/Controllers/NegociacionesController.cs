using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Negociaciones.Commands.CreateNegociacion;
using Miski.Application.Features.Negociaciones.Queries.GetNegociaciones;
using Miski.Shared.DTOs;

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
    public async Task<ActionResult<IEnumerable<NegociacionDto>>> GetNegociaciones(
        [FromQuery] int? proveedorId = null,
        [FromQuery] int? comisionistaId = null,
        [FromQuery] string? estado = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetNegociacionesQuery(proveedorId, comisionistaId, estado);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Crea una nueva negociación
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<NegociacionDto>> CreateNegociacion(
        [FromBody] CreateNegociacionDto request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateNegociacionCommand(request);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetNegociacionById), new { id = result.IdNegociacion }, result);
    }

    /// <summary>
    /// Obtiene una negociación por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<NegociacionDto>> GetNegociacionById(
        int id,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implementar GetNegociacionByIdQuery
        return NotFound("Endpoint en desarrollo");
    }

    /// <summary>
    /// Obtiene negociaciones pendientes de aprobación
    /// </summary>
    [HttpGet("pendientes-aprobacion")]
    public async Task<ActionResult<IEnumerable<NegociacionDto>>> GetPendientesAprobacion(
        CancellationToken cancellationToken = default)
    {
        var query = new GetNegociacionesQuery(Estado: "Pendiente");
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Aprueba una negociación
    /// </summary>
    [HttpPut("{id}/aprobar")]
    public async Task<ActionResult> AprobarNegociacion(
        int id,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implementar AprobarNegociacionCommand
        return NoContent();
    }
}