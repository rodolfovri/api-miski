using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Maestros.Vehiculo.Commands.CreateVehiculo;
using Miski.Application.Features.Maestros.Vehiculo.Commands.DeleteVehiculo;
using Miski.Application.Features.Maestros.Vehiculo.Commands.UpdateVehiculo;
using Miski.Application.Features.Maestros.Vehiculo.Queries.GetVehiculoById;
using Miski.Application.Features.Maestros.Vehiculo.Queries.GetVehiculos;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Api.Controllers.Maestros;

[ApiController]
[Route("api/maestros/vehiculos")]
[Tags("Maestros")]
[Authorize]
public class VehiculosController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehiculosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los vehículos
    /// </summary>
    /// <remarks>
    /// Permite filtrar por:
    /// - placa: Búsqueda parcial por placa
    /// - estado: Filtrar por estado (ACTIVO/INACTIVO)
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<VehiculoDto>>>> GetVehiculos(
        [FromQuery] string? placa = null,
        [FromQuery] string? estado = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetVehiculosQuery(placa, estado);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<IEnumerable<VehiculoDto>>.SuccessResult(
                result,
                "Vehículos obtenidos exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<VehiculoDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene un vehículo por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<VehiculoDto>>> GetVehiculoById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetVehiculoByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<VehiculoDto>.SuccessResult(
                result,
                "Vehículo obtenido exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<VehiculoDto>.ErrorResult(
                "Vehículo no encontrado",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<VehiculoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea un nuevo vehículo
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<VehiculoDto>>> CreateVehiculo(
        [FromBody] CreateVehiculoDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateVehiculoCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetVehiculoById),
                new { id = result.IdVehiculo },
                ApiResponse<VehiculoDto>.SuccessResult(
                    result,
                    "Vehículo creado exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<VehiculoDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<VehiculoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza un vehículo
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<VehiculoDto>>> UpdateVehiculo(
        int id,
        [FromBody] UpdateVehiculoDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdVehiculo)
            {
                return BadRequest(ApiResponse<VehiculoDto>.ErrorResult(
                    "ID inválido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petición"
                ));
            }

            var command = new UpdateVehiculoCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<VehiculoDto>.SuccessResult(
                result,
                "Vehículo actualizado exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<VehiculoDto>.ErrorResult(
                "Vehículo no encontrado",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<VehiculoDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<VehiculoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Elimina (inactiva) un vehículo
    /// </summary>
    /// <remarks>
    /// NOTA: No se puede eliminar un vehículo que tiene compras asociadas.
    /// El registro no se elimina físicamente, solo se marca como INACTIVO.
    /// </remarks>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteVehiculo(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteVehiculoCommand(id);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Vehículo eliminado exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Vehículo no encontrado",
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