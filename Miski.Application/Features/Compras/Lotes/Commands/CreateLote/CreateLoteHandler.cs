using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.Lotes.Commands.CreateLote;

public class CreateLoteHandler : IRequestHandler<CreateLoteCommand, LoteDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateLoteHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LoteDto> Handle(CreateLoteCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Lote;

        // Validar que la compra existe
        var compra = await _unitOfWork.Repository<Compra>()
            .GetByIdAsync(dto.IdCompra, cancellationToken);

        if (compra == null)
            throw new NotFoundException("Compra", dto.IdCompra);

        // Validar que el código no esté duplicado en la misma compra
        if (!string.IsNullOrEmpty(dto.Codigo))
        {
            var lotes = await _unitOfWork.Repository<Lote>().GetAllAsync(cancellationToken);
            if (lotes.Any(l => l.IdCompra == dto.IdCompra && l.Codigo == dto.Codigo))
            {
                throw new ValidationException($"Ya existe un lote con el código {dto.Codigo} en esta compra");
            }
        }

        // Crear el lote
        var lote = new Lote
        {
            IdCompra = dto.IdCompra,
            Peso = dto.Peso,
            Sacos = dto.Sacos,
            Codigo = dto.Codigo,
        };

        await _unitOfWork.Repository<Lote>().AddAsync(lote, cancellationToken);

        // Actualizar el MontoTotal en la Compra
        compra.MontoTotal = dto.MontoTotal;
        await _unitOfWork.Repository<Compra>().UpdateAsync(compra, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LoteDto>(lote);
    }
}
