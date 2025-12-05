using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Compras.CompraPagos.Commands.DefinirTipoPago;
using Miski.Application.Features.Compras.CompraPagos.Commands.RegistrarAbono;
using Miski.Application.Features.Compras.CompraPagos.Queries.GetHistorialPagos;
using Miski.Application.Features.Compras.CompraPagos.Queries.GetResumenPago;
using Miski.Application.Features.Compras.CompraPagos.Queries.GetComprasConEstadoPago;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Compras;
using Swashbuckle.AspNetCore.Annotations;

namespace Miski.Api.Controllers.Compras;

[Authorize]
[ApiController]
[Route("api/compras/historial-pago")]
[SwaggerTag("Gestion de pagos de compras")]
public class CompraPagosController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompraPagosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Define el tipo de pago (CONTADO o CREDITO) para una compra
    /// </summary>
    /// <param name="command">Datos del tipo de pago</param>
    /// <returns>Registro de pago creado</returns>
    [HttpPost("definir-tipo-pago")]
    [SwaggerOperation(
        Summary = "Definir tipo de pago",
        Description = "Define el tipo de pago (CONTADO o CREDITO) para una compra. Si es CONTADO se marca como pagado automáticamente. Si es CREDITO se puede registrar un adelanto inicial."
    )]
    [SwaggerResponse(200, "Tipo de pago definido exitosamente", typeof(ApiResponse<CompraPagoDto>))]
    [SwaggerResponse(400, "Datos inválidos o tipo de pago ya definido", typeof(ApiResponse))]
    [SwaggerResponse(404, "Compra no encontrada", typeof(ApiResponse))]
    public async Task<ActionResult<ApiResponse<CompraPagoDto>>> DefinirTipoPago(
        [FromBody] DefinirTipoPagoCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Registra un abono para una compra a crédito
    /// </summary>
    /// <param name="command">Datos del abono</param>
    /// <returns>Registro de pago creado</returns>
    [HttpPost("registrar-abono")]
    [SwaggerOperation(
        Summary = "Registrar abono",
        Description = "Registra un abono para una compra a crédito. Valida que no exceda el saldo pendiente y actualiza el estado de pago."
    )]
    [SwaggerResponse(200, "Abono registrado exitosamente", typeof(ApiResponse<CompraPagoDto>))]
    [SwaggerResponse(400, "Datos inválidos o abono excede el saldo", typeof(ApiResponse))]
    [SwaggerResponse(404, "Compra no encontrada", typeof(ApiResponse))]
    public async Task<ActionResult<ApiResponse<CompraPagoDto>>> RegistrarAbono(
        [FromBody] RegistrarAbonoCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene el historial de pagos de una compra
    /// </summary>
    /// <param name="idCompra">ID de la compra</param>
    /// <returns>Lista de pagos ordenada cronológicamente</returns>
    [HttpGet("historial/{idCompra:int}")]
    [SwaggerOperation(
        Summary = "Obtener historial de pagos",
        Description = "Obtiene todos los registros de pago de una compra específica, ordenados cronológicamente."
    )]
    [SwaggerResponse(200, "Historial de pagos obtenido exitosamente", typeof(List<CompraPagoDto>))]
    [SwaggerResponse(404, "Compra no encontrada")]
    public async Task<ActionResult<List<CompraPagoDto>>> GetHistorialPagos(int idCompra)
    {
        var query = new GetHistorialPagosQuery(idCompra);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene el resumen de pago de una compra
    /// </summary>
    /// <param name="idCompra">ID de la compra</param>
    /// <returns>Resumen detallado del estado de pago</returns>
    [HttpGet("resumen/{idCompra:int}")]
    [SwaggerOperation(
        Summary = "Obtener resumen de pago",
        Description = "Obtiene un resumen detallado del estado de pago de una compra, incluyendo monto total, abonado, saldo pendiente y todos los pagos realizados."
    )]
    [SwaggerResponse(200, "Resumen obtenido exitosamente", typeof(ResumenPagoDto))]
    [SwaggerResponse(404, "Compra no encontrada")]
    public async Task<ActionResult<ResumenPagoDto>> GetResumenPago(int idCompra)
    {
        var query = new GetResumenPagoQuery(idCompra);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene todas las compras con su estado de pago
    /// </summary>
    /// <param name="estadoPago">Filtro opcional por estado de pago (PENDIENTE, PARCIAL, PAGADO, SIN DEFINIR)</param>
    /// <param name="tipoPago">Filtro opcional por tipo de pago (CONTADO, CREDITO)</param>
    /// <returns>Lista de compras con su estado de pago</returns>
    [HttpGet("compras-con-estado")]
    [SwaggerOperation(
        Summary = "Obtener compras con estado de pago",
        Description = "Obtiene todas las compras con información resumida de su estado de pago. Permite filtrar por estado de pago y tipo de pago."
    )]
    [SwaggerResponse(200, "Lista obtenida exitosamente", typeof(List<CompraConEstadoPagoDto>))]
    public async Task<ActionResult<List<CompraConEstadoPagoDto>>> GetComprasConEstadoPago(
        [FromQuery] string? estadoPago = null,
        [FromQuery] string? tipoPago = null)
    {
        var query = new GetComprasConEstadoPagoQuery(estadoPago, tipoPago);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
