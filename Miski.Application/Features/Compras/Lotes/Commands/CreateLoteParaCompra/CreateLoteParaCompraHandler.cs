using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.Lotes.Commands.CreateLoteParaCompra;

public class CreateLoteParaCompraHandler : IRequestHandler<CreateLoteParaCompraCommand, LoteDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateLoteParaCompraHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LoteDto> Handle(CreateLoteParaCompraCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Lote;

        // 1. Validar que la compra existe
        var compra = await _unitOfWork.Repository<Compra>()
            .GetByIdAsync(request.IdCompra, cancellationToken);

        if (compra == null)
            throw new NotFoundException("Compra", request.IdCompra);

        // 2. Validar que la compra NO tenga un lote ya asignado (relación 1:1)
        if (compra.IdLote.HasValue)
        {
            throw new ValidationException($"La compra ya tiene un lote asignado.");
        }

        // 3. Validar que la compra esté ACTIVA
        if (compra.Estado != "ACTIVO")
        {
            throw new ValidationException($"Solo se pueden asignar lotes a compras con estado ACTIVO. Estado actual: {compra.Estado}");
        }

        // 4. Validar que el código no esté duplicado si se proporciona
        if (!string.IsNullOrEmpty(dto.Codigo))
        {
            var lotes = await _unitOfWork.Repository<Lote>().GetAllAsync(cancellationToken);
            if (lotes.Any(l => l.Codigo == dto.Codigo))
            {
                throw new ValidationException($"Ya existe un lote con el código {dto.Codigo}");
            }
        }

        // 5. Crear el lote
        var lote = new Lote
        {
            Peso = dto.Peso,
            Sacos = dto.Sacos,
            Codigo = dto.Codigo,
            Comision = dto.Comision,
            Observacion = dto.Observacion
        };

        await _unitOfWork.Repository<Lote>().AddAsync(lote, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 6. Asignar el lote a la compra y actualizar el monto total
        compra.IdLote = lote.IdLote;
        compra.MontoTotal = request.MontoTotal;

        await _unitOfWork.Repository<Compra>().UpdateAsync(compra, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 7. Cargar la relación inversa para el DTO
        lote.Compra = compra;

        return _mapper.Map<LoteDto>(lote);
    }
}
