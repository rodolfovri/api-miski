using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.CompraPagos.Commands.RegistrarAbono;

public class RegistrarAbonoHandler : IRequestHandler<RegistrarAbonoCommand, ApiResponse<CompraPagoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegistrarAbonoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CompraPagoDto>> Handle(RegistrarAbonoCommand request, CancellationToken cancellationToken)
    {
        // Validar que la compra existe
        var compra = await _unitOfWork.Repository<Compra>().GetByIdAsync(request.IdCompra, cancellationToken);
        if (compra == null)
            throw new NotFoundException("Compra", request.IdCompra);

        // Validar que el tipo de pago ha sido definido
        if (string.IsNullOrEmpty(compra.TipoPago))
            throw new DomainException("Debe definir el tipo de pago primero antes de registrar un abono");

        // Validar que el tipo de pago es CREDITO
        if (compra.TipoPago != "CREDITO")
            throw new DomainException($"Solo se pueden registrar abonos para compras a CREDITO. Esta compra es de tipo {compra.TipoPago}");

        // Obtener todos los pagos de esta compra
        var todosPagos = await _unitOfWork.Repository<CompraPago>().GetAllAsync(cancellationToken);
        var pagosCompra = todosPagos.Where(p => p.IdCompra == request.IdCompra).ToList();

        // Calcular saldo actual
        var totalAbonado = pagosCompra.Sum(p => p.MontoAcuenta ?? 0);
        var saldoAnterior = (compra.MontoTotal ?? 0) - totalAbonado;
        var nuevoSaldo = saldoAnterior - request.MontoAbono;

        // Validar que no exceda el monto total
        if (nuevoSaldo < 0)
            throw new DomainException($"El abono excede el saldo pendiente. Saldo actual: {saldoAnterior:F2}");

        // Crear el nuevo registro de abono
        var nuevoAbono = new CompraPago
        {
            IdCompra = request.IdCompra,
            TipoPago = compra.TipoPago,
            MontoAcuenta = request.MontoAbono,
            Saldo = nuevoSaldo,
            EstadoPago = nuevoSaldo == 0 ? "PAGADO" : "PARCIAL",
            FPago = request.FechaPago ?? DateTime.UtcNow,
            Observacion = request.Observacion,
            FRegistro = DateTime.UtcNow
        };

        // Guardar el abono
        await _unitOfWork.Repository<CompraPago>().AddAsync(nuevoAbono, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var compraPagoDto = _mapper.Map<CompraPagoDto>(nuevoAbono);
        compraPagoDto.CompraSerie = compra.Serie;
        compraPagoDto.CompraMontoTotal = compra.MontoTotal;

        return ApiResponse<CompraPagoDto>.SuccessResult(
            compraPagoDto,
            nuevoSaldo == 0 
                ? "Abono registrado exitosamente. La compra ha sido pagada completamente" 
                : $"Abono registrado exitosamente. Saldo pendiente: {nuevoSaldo:F2}"
        );
    }
}
