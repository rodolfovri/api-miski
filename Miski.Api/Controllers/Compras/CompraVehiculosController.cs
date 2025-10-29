using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Compras.CompraVehiculos.Commands.CreateCompraVehiculo;
using Miski.Application.Features.Compras.CompraVehiculos.Commands.UpdateCompraVehiculo;
using Miski.Application.Features.Compras.CompraVehiculos.Queries.GetCompraVehiculos;
using Miski.Application.Features.Compras.CompraVehiculos.Queries.GetCompraVehiculoById;
using Miski.Application.Features.Compras.CompraVehiculos.Queries.GetCompraVehiculoConDisponibles;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Compras;

namespace Miski.Api.Controllers.Compras;

[ApiController]
[Route("api/compras/asignacion-vehiculos")]
[Tags("Compras")]
[Authorize]
public class CompraVehiculosController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompraVehiculosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todas las asignaciones de compras a veh�culos con filtros opcionales
    /// </summary>
    /// <remarks>
    /// Permite filtrar por:
    /// - idVehiculo: Filtrar por veh�culo
    /// - guiaRemision: B�squeda parcial por gu�a de remisi�n
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CompraVehiculoDto>>>> GetCompraVehiculos(
        [FromQuery] int? idVehiculo = null,
        [FromQuery] string? guiaRemision = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetCompraVehiculosQuery(idVehiculo, guiaRemision);
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<CompraVehiculoDto>>.SuccessResult(
                result, 
                "Asignaciones de compras a veh�culos obtenidas exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<CompraVehiculoDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene una asignaci�n de compra a veh�culo por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CompraVehiculoDto>>> GetCompraVehiculoById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetCompraVehiculoByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<CompraVehiculoDto>.SuccessResult(
                result,
                "Asignaci�n de compra a veh�culo obtenida exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<CompraVehiculoDto>.ErrorResult(
                "Asignaci�n de compra a veh�culo no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CompraVehiculoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea una nueva asignaci�n de compras a veh�culo
    /// </summary>
    /// <remarks>
    /// Permite asignar m�ltiples compras a un veh�culo con una gu�a de remisi�n.
    /// 
    /// Campos requeridos:
    /// - IdVehiculo: ID del veh�culo al que se asignan las compras
    /// - GuiaRemision: N�mero de gu�a de remisi�n (�nico, m�x. 50 caracteres)
    /// - IdCompras: Lista de IDs de compras a asignar (m�nimo 1)
    /// 
    /// Validaciones:
    /// - El veh�culo debe existir
    /// - Todas las compras deben existir y estar en estado ACTIVO
    /// - Las compras no deben estar asignadas previamente a otro veh�culo
    /// - La gu�a de remisi�n no debe estar duplicada
    /// 
    /// Ejemplo de request:
    /// {
    ///   "idVehiculo": 1,
    ///   "guiaRemision": "GR-2024-001",
    ///   "idCompras": [1, 2, 3]
    /// }
    /// </remarks>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CompraVehiculoDto>>> CreateCompraVehiculo(
        [FromBody] CreateCompraVehiculoDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateCompraVehiculoCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetCompraVehiculoById), 
                new { id = result.IdCompraVehiculo }, 
                ApiResponse<CompraVehiculoDto>.SuccessResult(
                    result, 
                    "Asignaci�n de compras a veh�culo creada exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<CompraVehiculoDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<CompraVehiculoDto>.ErrorResult(
                "Entidad relacionada no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CompraVehiculoDto>.ErrorResult(
                "Error interno del servidor", 
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza una asignaci�n de compras a veh�culo
    /// </summary>
    /// <remarks>
    /// Permite modificar el veh�culo, la gu�a de remisi�n y las compras asignadas.
    /// 
    /// Campos requeridos:
    /// - IdCompraVehiculo: ID de la asignaci�n a actualizar
    /// - IdVehiculo: ID del veh�culo
    /// - GuiaRemision: N�mero de gu�a de remisi�n (�nico, m�x. 50 caracteres)
    /// - IdCompras: Lista de IDs de compras a asignar (m�nimo 1)
    /// 
    /// Validaciones:
    /// - La asignaci�n debe existir
    /// - El veh�culo debe existir
    /// - Todas las compras deben existir y estar en estado ACTIVO
    /// - Las compras no deben estar asignadas a OTRO veh�culo (permite reasignar en el mismo)
    /// - La gu�a de remisi�n no debe estar duplicada (excepto la actual)
    /// 
    /// Nota: Los detalles anteriores se eliminan y se crean nuevos con las compras indicadas
    /// 
    /// Ejemplo de request:
    /// {
    ///   "idCompraVehiculo": 1,
    ///   "idVehiculo": 2,
    ///   "guiaRemision": "GR-2024-001-MOD",
    ///   "idCompras": [1, 2, 4, 5]
    /// }
    /// </remarks>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CompraVehiculoDto>>> UpdateCompraVehiculo(
        int id,
        [FromBody] UpdateCompraVehiculoDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdCompraVehiculo)
            {
                return BadRequest(ApiResponse<CompraVehiculoDto>.ErrorResult(
                    "ID inv�lido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petici�n"
                ));
            }

            var command = new UpdateCompraVehiculoCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<CompraVehiculoDto>.SuccessResult(
                result,
                "Asignaci�n de compras a veh�culo actualizada exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<CompraVehiculoDto>.ErrorResult(
                "Asignaci�n de compra a veh�culo no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<CompraVehiculoDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CompraVehiculoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene una asignaci�n de compra a veh�culo por ID con compras asignadas y disponibles
    /// </summary>
    /// <remarks>
    /// Similar a GetCompraVehiculoById pero incluye:
    /// - Compras asignadas a este CompraVehiculo (Asignado = true)
    /// - Compras ACTIVAS sin asignar a ning�n veh�culo (Asignado = false)
    /// 
    /// �til para gestionar qu� compras est�n asignadas y cu�les est�n disponibles para asignar.
    /// </remarks>
    [HttpGet("{id}/con-disponibles")]
    public async Task<ActionResult<ApiResponse<CompraVehiculoConDisponiblesDto>>> GetCompraVehiculoConDisponibles(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetCompraVehiculoConDisponiblesQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<CompraVehiculoConDisponiblesDto>.SuccessResult(
                result,
                "Asignaci�n de compra a veh�culo con compras disponibles obtenida exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<CompraVehiculoConDisponiblesDto>.ErrorResult(
                "Asignaci�n de compra a veh�culo no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CompraVehiculoConDisponiblesDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }
}
