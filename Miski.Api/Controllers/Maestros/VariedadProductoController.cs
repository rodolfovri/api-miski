using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Maestros.VariedadProducto.Commands.CreateVariedad;
using Miski.Application.Features.Maestros.VariedadProducto.Commands.UpdateVariedad;
using Miski.Application.Features.Maestros.VariedadProducto.Commands.DeleteVariedad;
using Miski.Application.Features.Maestros.VariedadProducto.Queries.GetVariedades;
using Miski.Application.Features.Maestros.VariedadProducto.Queries.GetVariedadById;
using Miski.Application.Features.Maestros.VariedadProducto.Queries.GetKardex;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Api.Controllers.Maestros;

[ApiController]
[Route("api/maestros/variedad-producto")]
[Tags("Maestros")]
[Authorize]
public class VariedadProductoController : ControllerBase
{
    private readonly IMediator _mediator;

    public VariedadProductoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todas las variedades de productos
    /// </summary>
    /// <remarks>
    /// Permite filtrar por:
    /// - nombre: Búsqueda parcial por nombre
    /// - codigo: Búsqueda parcial por código
    /// - idProducto: Filtrar por producto
    /// - estado: Filtrar por estado (ACTIVO, INACTIVO)
    /// - idUbicacion: Filtrar por ubicación/planta para obtener el stock específico
    /// 
    /// **Stock:**
    /// - Si se proporciona `idUbicacion`, el campo `stockKg` mostrará el stock disponible en esa ubicación
    /// - Si NO se proporciona `idUbicacion`, el campo `stockKg` será 0
    /// - Si no existe stock para una variedad en la ubicación especificada, el campo `stockKg` será 0
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<VariedadProductoDto>>>> GetVariedades(
        [FromQuery] string? nombre = null,
        [FromQuery] string? codigo = null,
        [FromQuery] int? idProducto = null,
        [FromQuery] string? estado = null,
        [FromQuery] int? idUbicacion = null,  // ? NUEVO
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetVariedadesProductoQuery(nombre, codigo, idProducto, estado, idUbicacion);
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<VariedadProductoDto>>.SuccessResult(
                result,
                "Variedades obtenidas exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<VariedadProductoDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene una variedad por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<VariedadProductoDto>>> GetVariedadById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetVariedadProductoByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<VariedadProductoDto>.SuccessResult(
                result,
                "Variedad obtenida exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<VariedadProductoDto>.ErrorResult(
                "Variedad no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<VariedadProductoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea una nueva variedad de producto
    /// </summary>
    /// <remarks>
    /// Este endpoint acepta multipart/form-data.
    /// Campo opcional:
    /// - FichaTecnica: Archivo PDF con información técnica de la variedad
    /// </remarks>
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ApiResponse<VariedadProductoDto>>> CreateVariedad(
        [FromForm] CreateVariedadProductoDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateVariedadProductoCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetVariedadById),
                new { id = result.IdVariedadProducto },
                ApiResponse<VariedadProductoDto>.SuccessResult(
                    result,
                    "Variedad creada exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<VariedadProductoDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<VariedadProductoDto>.ErrorResult(
                "Entidad relacionada no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<VariedadProductoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza una variedad de producto
    /// </summary>
    /// <remarks>
    /// Este endpoint acepta multipart/form-data.
    /// Campo opcional para actualizar:
    /// - FichaTecnica: Archivo PDF (reemplaza la ficha técnica anterior)
    /// </remarks>
    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ApiResponse<VariedadProductoDto>>> UpdateVariedad(
        int id,
        [FromForm] UpdateVariedadProductoDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdVariedadProducto)
            {
                return BadRequest(ApiResponse<VariedadProductoDto>.ErrorResult(
                    "ID inválido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petición"
                ));
            }

            var command = new UpdateVariedadProductoCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<VariedadProductoDto>.SuccessResult(
                result,
                "Variedad actualizada exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<VariedadProductoDto>.ErrorResult(
                "Variedad no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<VariedadProductoDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<VariedadProductoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Elimina una variedad de producto (cambio de estado a INACTIVO)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteVariedad(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteVariedadProductoCommand(id);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Variedad eliminada exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Variedad no encontrada",
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

    /// <summary>
    /// Obtiene el Kardex de una variedad de producto
    /// </summary>
    /// <remarks>
    /// Retorna el detalle de todos los movimientos de almacén (ingresos y salidas) 
    /// de una variedad de producto específica en un rango de fechas.
    /// 
    /// **Información incluida:**
    /// - Stock inicial (antes del rango de fechas)
    /// - Movimientos detallados con tipo de operación (INGRESO/SALIDA)
    /// - Tipo de stock de cada movimiento (MATERIA_PRIMA/PRODUCTO_TERMINADO)
    /// - Cantidades en kg y número de sacos por movimiento
    /// - Saldo acumulado después de cada movimiento
    /// - Stock final (al final del rango de fechas)
    /// - Usuario que realizó cada movimiento
    /// - Ubicación de cada movimiento
    /// - Código de lote asociado (si aplica)
    /// 
    /// **Parámetros:**
    /// - idVariedadProducto: ID de la variedad de producto (requerido)
    /// - fechaDesde: Fecha inicial del rango (requerido, formato: yyyy-MM-dd)
    /// - fechaHasta: Fecha final del rango (requerido, formato: yyyy-MM-dd)
    /// - tipoStock: Filtrar por tipo de stock (opcional: MATERIA_PRIMA o PRODUCTO_TERMINADO)
    ///   * Si no se envía, retorna todos los movimientos sin filtrar por tipo
    ///   * Si se envía "MATERIA_PRIMA", solo movimientos de materia prima
    ///   * Si se envía "PRODUCTO_TERMINADO", solo movimientos de producto terminado
    /// 
    /// **Ejemplos de uso:**
    /// - Todos los movimientos: GET /api/maestros/variedad-producto/5/kardex?fechaDesde=2024-01-01&amp;fechaHasta=2024-12-31
    /// - Solo materia prima: GET /api/maestros/variedad-producto/5/kardex?fechaDesde=2024-01-01&amp;fechaHasta=2024-12-31&amp;tipoStock=MATERIA_PRIMA
    /// - Solo producto terminado: GET /api/maestros/variedad-producto/5/kardex?fechaDesde=2024-01-01&amp;fechaHasta=2024-12-31&amp;tipoStock=PRODUCTO_TERMINADO
    /// </remarks>
    [HttpGet("{id}/kardex")]
    public async Task<ActionResult<ApiResponse<KardexVariedadProductoDto>>> GetKardex(
        int id,
        [FromQuery] DateTime fechaDesde,
        [FromQuery] DateTime fechaHasta,
        [FromQuery] string? tipoStock = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetKardexVariedadProductoQuery(id, fechaDesde, fechaHasta, tipoStock);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<KardexVariedadProductoDto>.SuccessResult(
                result,
                "Kardex obtenido exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<KardexVariedadProductoDto>.ErrorResult(
                "Variedad no encontrada",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<KardexVariedadProductoDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<KardexVariedadProductoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }
}
