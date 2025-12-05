using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.CompraPagos.Commands.DefinirTipoPago;

public class DefinirTipoPagoHandler : IRequestHandler<DefinirTipoPagoCommand, ApiResponse<CompraPagoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DefinirTipoPagoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CompraPagoDto>> Handle(DefinirTipoPagoCommand request, CancellationToken cancellationToken)
    {
        // Validar que la compra existe
        var compra = await _unitOfWork.Repository<Compra>().GetByIdAsync(request.IdCompra, cancellationToken);
        if (compra == null)
            throw new NotFoundException("Compra", request.IdCompra);

        // Validar que el tipo de pago no ha sido definido previamente
        if (!string.IsNullOrEmpty(compra.TipoPago))
            throw new DomainException($"El tipo de pago ya fue definido como {compra.TipoPago} para esta compra");

        // Actualizar el tipo de pago en la compra
        compra.TipoPago = request.TipoPago;
        await _unitOfWork.Repository<Compra>().UpdateAsync(compra, cancellationToken);

        // Crear el primer registro en CompraPago
        var compraPago = new CompraPago
        {
            IdCompra = request.IdCompra,
            TipoPago = request.TipoPago,
            DiasCredito = request.DiasCredito,
            FPago = request.FechaPago,
            Observacion = request.Observacion,
            FRegistro = DateTime.UtcNow
        };

        // Configurar según el tipo de pago
        if (request.TipoPago == "CONTADO")
        {
            compraPago.MontoAcuenta = compra.MontoTotal;
            compraPago.Saldo = 0m;
            compraPago.EstadoPago = "PAGADO";
            compraPago.FPago = request.FechaPago ?? DateTime.UtcNow;
        }
        else if (request.TipoPago == "CREDITO")
        {
            // Si hay adelanto inicial
            if (request.MontoAdelanto.HasValue && request.MontoAdelanto.Value > 0)
            {
                // Validar que el adelanto no exceda el monto total
                if (request.MontoAdelanto.Value > compra.MontoTotal)
                    throw new DomainException($"El monto de adelanto ({request.MontoAdelanto.Value}) no puede ser mayor al monto total de la compra ({compra.MontoTotal})");

                compraPago.MontoAcuenta = request.MontoAdelanto.Value;
                compraPago.Saldo = compra.MontoTotal - request.MontoAdelanto.Value;
                compraPago.EstadoPago = "PARCIAL";
                compraPago.FPago = request.FechaPago ?? DateTime.UtcNow;
            }
            else
            {
                // Sin adelanto, solo se define el crédito
                compraPago.MontoAcuenta = 0m;
                compraPago.Saldo = compra.MontoTotal;
                compraPago.EstadoPago = "PENDIENTE";
            }
        }

        // Guardar el registro de pago
        await _unitOfWork.Repository<CompraPago>().AddAsync(compraPago, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var compraPagoDto = _mapper.Map<CompraPagoDto>(compraPago);
        compraPagoDto.CompraSerie = compra.Serie;
        compraPagoDto.CompraMontoTotal = compra.MontoTotal;

        return ApiResponse<CompraPagoDto>.SuccessResult(
            compraPagoDto,
            $"Tipo de pago {request.TipoPago} definido exitosamente"
        );
    }
}
