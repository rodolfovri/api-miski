using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;

namespace Miski.Application.Features.Compras.CompraPagos.Queries.GetComprasConEstadoPago;

public class GetComprasConEstadoPagoHandler : IRequestHandler<GetComprasConEstadoPagoQuery, List<CompraConEstadoPagoDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetComprasConEstadoPagoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<CompraConEstadoPagoDto>> Handle(GetComprasConEstadoPagoQuery request, CancellationToken cancellationToken)
    {
        var compras = await _unitOfWork.Repository<Compra>().GetAllAsync(cancellationToken);
        var todosPagos = await _unitOfWork.Repository<CompraPago>().GetAllAsync(cancellationToken);

        var resultado = new List<CompraConEstadoPagoDto>();

        foreach (var compra in compras)
        {
            var pagosCompra = todosPagos.Where(p => p.IdCompra == compra.IdCompra).ToList();
            var totalAbonado = pagosCompra.Sum(p => p.MontoAcuenta ?? 0);
            var saldoPendiente = (compra.MontoTotal ?? 0) - totalAbonado;
            var ultimoPago = pagosCompra.OrderByDescending(p => p.FPago).FirstOrDefault();
            var diasCredito = pagosCompra.FirstOrDefault()?.DiasCredito;

            // Determinar estado de pago
            string estadoPago = "SIN DEFINIR";
            if (!string.IsNullOrEmpty(compra.TipoPago))
            {
                if (saldoPendiente == 0)
                    estadoPago = "PAGADO";
                else if (totalAbonado > 0)
                    estadoPago = "PARCIAL";
                else
                    estadoPago = "PENDIENTE";
            }

            var compraDto = new CompraConEstadoPagoDto
            {
                IdCompra = compra.IdCompra,
                Serie = compra.Serie,
                MontoTotal = compra.MontoTotal,
                TipoPago = compra.TipoPago,
                EstadoPago = estadoPago,
                TotalAbonado = totalAbonado,
                SaldoPendiente = saldoPendiente,
                DiasCredito = diasCredito,
                FechaUltimoPago = ultimoPago?.FPago,
                FRegistro = compra.FRegistro,
                TotalPagos = pagosCompra.Count
            };

            resultado.Add(compraDto);
        }

        // Aplicar filtros
        if (!string.IsNullOrEmpty(request.EstadoPago))
        {
            resultado = resultado.Where(c => c.EstadoPago == request.EstadoPago).ToList();
        }

        if (!string.IsNullOrEmpty(request.TipoPago))
        {
            resultado = resultado.Where(c => c.TipoPago == request.TipoPago).ToList();
        }

        return resultado.OrderByDescending(c => c.FRegistro).ToList();
    }
}
