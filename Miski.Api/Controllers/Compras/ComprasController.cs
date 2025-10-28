using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Compras.Compras.Queries.GetCompras;
using Miski.Application.Features.Compras.Compras.Queries.GetCompraById;
using Miski.Application.Features.Compras.Lotes.Commands.CreateLote;
using Miski.Application.Features.Compras.Lotes.Commands.UpdateLote;
using Miski.Application.Features.Compras.Lotes.Commands.DeleteLote;
using Miski.Application.Features.Compras.Lotes.Queries.GetLotes;
using Miski.Application.Features.Compras.Lotes.Queries.GetLoteById;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Compras;

namespace Miski.Api.Controllers.Compras;

[ApiController]
[Route("api/compras")]
[Tags("Compras")]
[Authorize]
public class ComprasController : ControllerBase
{
    private readonly IMediator _mediator;

    public ComprasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todas las compras con su informaci�n de negociaci�n y lotes
    /// </summary>
    /// <remarks>
    /// Permite filtrar por:
    /// - estado: Filtrar por estado (EN PROCESO/COMPLETADA)
    /// - idNegociacion: Filtrar por ID de negociaci�n
    /// 
    /// Incluye autom�ticamente:
    /// - Informaci�n del proveedor y comisionista de la negociaci�n
    /// - Lista de lotes asociados a cada compra
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CompraDto>>>> GetCompras(
        [FromQuery] string? estado = null,
        [FromQuery] int? idNegociacion = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetComprasQuery(estado, idNegociacion);
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<CompraDto>>.SuccessResult(
                result,
                "Compras obtenidas exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<CompraDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene una compra por ID con toda su informaci�n
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CompraDto>>> GetCompraById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetCompraByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<CompraDto>.SuccessResult(
                result,
                "Compra obtenida exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<CompraDto>.ErrorResult(
                "Compra no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CompraDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene todos los lotes
    /// </summary>
    /// <remarks>
    /// Permite filtrar por:
    /// - idCompra: Filtrar por compra espec�fica
    /// - codigo: B�squeda parcial por c�digo
    /// </remarks>
    [HttpGet("lotes")]
    public async Task<ActionResult<ApiResponse<IEnumerable<LoteDto>>>> GetLotes(
        [FromQuery] int? idCompra = null,
        [FromQuery] string? codigo = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetLotesQuery(idCompra, codigo);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<IEnumerable<LoteDto>>.SuccessResult(
                result,
                "Lotes obtenidos exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<LoteDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene un lote por ID
    /// </summary>
    [HttpGet("lotes/{id}")]
    public async Task<ActionResult<ApiResponse<LoteDto>>> GetLoteById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetLoteByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<LoteDto>.SuccessResult(
                result,
                "Lote obtenido exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<LoteDto>.ErrorResult(
                "Lote no encontrado",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<LoteDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea un nuevo lote para una compra
    /// </summary>
    [HttpPost("lotes")]
    public async Task<ActionResult<ApiResponse<LoteDto>>> CreateLote(
        [FromBody] CreateLoteDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateLoteCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetLoteById),
                new { id = result.IdLote },
                ApiResponse<LoteDto>.SuccessResult(
                    result,
                    "Lote creado exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<LoteDto>.ErrorResult(
                "Entidad no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<LoteDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<LoteDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza un lote
    /// </summary>
    [HttpPut("lotes/{id}")]
    public async Task<ActionResult<ApiResponse<LoteDto>>> UpdateLote(
        int id,
        [FromBody] UpdateLoteDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdLote)
            {
                return BadRequest(ApiResponse<LoteDto>.ErrorResult(
                    "ID inv�lido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petici�n"
                ));
            }

            var command = new UpdateLoteCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<LoteDto>.SuccessResult(
                result,
                "Lote actualizado exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<LoteDto>.ErrorResult(
                "Lote no encontrado",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<LoteDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<LoteDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Elimina un lote
    /// </summary>
    /// <remarks>
    /// NOTA: No se puede eliminar un lote que tiene llegadas de planta asociadas.
    /// </remarks>
    [HttpDelete("lotes/{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteLote(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteLoteCommand(id);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Lote eliminado exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Lote no encontrado",
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