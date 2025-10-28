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
    /// Obtiene todos los veh�culos
    /// </summary>
    /// <remarks>
    /// Permite filtrar por:
    /// - placa: B�squeda parcial por placa
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
                "Veh�culos obtenidos exitosamente"
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
    /// Obtiene un veh�culo por ID
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
                "Veh�culo obtenido exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<VehiculoDto>.ErrorResult(
                "Veh�culo no encontrado",
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
    /// Crea un nuevo veh�culo
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
                    "Veh�culo creado exitosamente"
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
    /// Actualiza un veh�culo
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
                    "ID inv�lido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petici�n"
                ));
            }

            var command = new UpdateVehiculoCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<VehiculoDto>.SuccessResult(
                result,
                "Veh�culo actualizado exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<VehiculoDto>.ErrorResult(
                "Veh�culo no encontrado",
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
    /// Elimina (inactiva) un veh�culo
    /// </summary>
    /// <remarks>
    /// NOTA: No se puede eliminar un veh�culo que tiene compras asociadas.
    /// El registro no se elimina f�sicamente, solo se marca como INACTIVO.
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

            return Ok(ApiResponse.SuccessResult("Veh�culo eliminado exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Veh�culo no encontrado",
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