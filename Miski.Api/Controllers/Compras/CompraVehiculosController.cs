using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Compras.CompraVehiculos.Commands.CreateCompraVehiculo;
using Miski.Application.Features.Compras.CompraVehiculos.Commands.UpdateCompraVehiculo;
using Miski.Application.Features.Compras.CompraVehiculos.Commands.EliminarCompraDeVehiculo;
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
    /// Obtiene todas las asignaciones de compras a vehículos con filtros opcionales
    /// </summary>
    /// <remarks>
    /// Permite filtrar por:
    /// - idVehiculo: Filtrar por vehículo
    /// - guiaRemision: Búsqueda parcial por guía de remisión
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CompraVehiculoDto>>>> GetCompraVehiculos(
        [FromQuery] DateTime? fechaDesde = null,
        [FromQuery] DateTime? fechaHasta = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetCompraVehiculosQuery(fechaDesde, fechaHasta);
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<CompraVehiculoDto>>.SuccessResult(
                result, 
                "Asignaciones de compras a vehículos obtenidas exitosamente"
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
    /// Obtiene una asignación de compra a vehículo por ID
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
                "Asignación de compra a vehículo obtenida exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<CompraVehiculoDto>.ErrorResult(
                "Asignación de compra a vehículo no encontrada",
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
    /// Crea una nueva asignación de compras a vehículo
    /// </summary>
    /// <remarks>
    /// Permite asignar múltiples compras a un vehículo con una guía de remisión.
    /// 
    /// Campos requeridos:
    /// - IdVehiculo: ID del vehículo al que se asignan las compras
    /// - GuiaRemision: Número de guía de remisión (único, máx. 50 caracteres)
    /// - IdCompras: Lista de IDs de compras a asignar (mínimo 1)
    /// 
    /// Validaciones:
    /// - El vehículo debe existir
    /// - Todas las compras deben existir y estar en estado ACTIVO
    /// - Las compras no deben estar asignadas previamente a otro vehículo
    /// - La guía de remisión no debe estar duplicada
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
                    "Asignación de compras a vehículo creada exitosamente"
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
    /// Actualiza una asignación de compras a vehículo
    /// </summary>
    /// <remarks>
    /// Permite modificar el vehículo, la guía de remisión y las compras asignadas.
    /// 
    /// Campos requeridos:
    /// - IdCompraVehiculo: ID de la asignación a actualizar
    /// - IdVehiculo: ID del vehículo
    /// - GuiaRemision: Número de guía de remisión (único, máx. 50 caracteres)
    /// - IdCompras: Lista de IDs de compras a asignar (mínimo 1)
    /// 
    /// Validaciones:
    /// - La asignación debe existir
    /// - El vehículo debe existir
    /// - Todas las compras deben existir y estar en estado ACTIVO
    /// - Las compras no deben estar asignadas a OTRO vehículo (permite reasignar en el mismo)
    /// - La guía de remisión no debe estar duplicada (excepto la actual)
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
                    "ID inválido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petición"
                ));
            }

            var command = new UpdateCompraVehiculoCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<CompraVehiculoDto>.SuccessResult(
                result,
                "Asignación de compras a vehículo actualizada exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<CompraVehiculoDto>.ErrorResult(
                "Asignación de compra a vehículo no encontrada",
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
    /// Obtiene una asignación de compra a vehículo por ID con compras asignadas y disponibles
    /// </summary>
    /// <remarks>
    /// Similar a GetCompraVehiculoById pero incluye:
    /// - Compras asignadas a este CompraVehiculo (Asignado = true)
    /// - Compras ACTIVAS sin asignar a ningún vehículo (Asignado = false)
    /// 
    /// Útil para gestionar qué compras están asignadas y cuáles están disponibles para asignar.
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
                "Asignación de compra a vehículo con compras disponibles obtenida exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<CompraVehiculoConDisponiblesDto>.ErrorResult(
                "Asignación de compra a vehículo no encontrada",
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

    /// <summary>
    /// Elimina una compra asociada a un vehículo
    /// </summary>
    /// <remarks>
    /// Elimina la asociación (CompraVehiculoDetalle) entre una compra y un vehículo.
    /// 
    /// Este endpoint es útil cuando:
    /// - Necesitas anular una compra que ya está asignada a un vehículo
    /// - Primero debes eliminar la asociación, luego puedes anular la compra
    /// 
    /// Validaciones implementadas:
    /// 1. ? El CompraVehiculoDetalle debe existir
    /// 2. ? El CompraVehiculo asociado debe estar en estado "ACTIVO" (no "ENTREGADO")
    /// 3. ? La Compra NO debe tener EstadoRecepcion = "RECEPCIONADO"
    /// 4. ? La Compra NO debe tener llegadas a planta registradas
    /// 
    /// Después de eliminar:
    /// - Se elimina el CompraVehiculoDetalle
    /// - Se actualiza Compra.EstadoRecepcion = null
    /// 
    /// Flujo recomendado para anular una compra:
    /// 1. DELETE /api/compras/asignacion-vehiculos/detalles/{idCompraVehiculoDetalle}
    /// 2. DELETE /api/compras/{idCompra} (anular la compra)
    /// </remarks>
    [HttpDelete("detalles/{idCompraVehiculoDetalle}")]
    public async Task<ActionResult<ApiResponse>> EliminarCompraDeVehiculo(
        int idCompraVehiculoDetalle,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new EliminarCompraDeVehiculoCommand(idCompraVehiculoDetalle);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Compra desasociada del vehículo exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Entidad no encontrada",
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
