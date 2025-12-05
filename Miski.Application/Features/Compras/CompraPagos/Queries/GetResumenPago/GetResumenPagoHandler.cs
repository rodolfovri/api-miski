using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.CompraPagos.Queries.GetResumenPago;

public class GetResumenPagoHandler : IRequestHandler<GetResumenPagoQuery, ResumenPagoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetResumenPagoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResumenPagoDto> Handle(GetResumenPagoQuery request, CancellationToken cancellationToken)
    {
        // Validar que la compra existe
        var compra = await _unitOfWork.Repository<Compra>().GetByIdAsync(request.IdCompra, cancellationToken);
        if (compra == null)
            throw new NotFoundException("Compra", request.IdCompra);

        // Obtener todos los pagos de la compra
        var todosPagos = await _unitOfWork.Repository<CompraPago>().GetAllAsync(cancellationToken);
        var pagosCompra = todosPagos
            .Where(p => p.IdCompra == request.IdCompra)
            .OrderBy(p => p.FRegistro)
            .ToList();

        // Calcular totales
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

        var resumen = new ResumenPagoDto
        {
            IdCompra = request.IdCompra,
            CompraSerie = compra.Serie,
            TipoPago = compra.TipoPago,
            MontoTotal = compra.MontoTotal,
            TotalAbonado = totalAbonado,
            SaldoPendiente = saldoPendiente,
            EstadoPago = estadoPago,
            DiasCredito = diasCredito,
            FechaUltimoPago = ultimoPago?.FPago,
            TotalPagos = pagosCompra.Count,
            Pagos = pagosCompra.Select(p =>
            {
                var dto = _mapper.Map<Shared.DTOs.Compras.CompraPagoDto>(p);
                dto.CompraSerie = compra.Serie;
                dto.CompraMontoTotal = compra.MontoTotal;
                return dto;
            }).ToList()
        };

        return resumen;
    }
}
