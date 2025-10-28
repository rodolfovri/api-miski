using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.Lotes.Commands.UpdateLote;

public class UpdateLoteHandler : IRequestHandler<UpdateLoteCommand, LoteDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateLoteHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LoteDto> Handle(UpdateLoteCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Lote;

        var lote = await _unitOfWork.Repository<Lote>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (lote == null)
            throw new NotFoundException("Lote", request.Id);

        // Validar que la compra existe
        var compra = await _unitOfWork.Repository<Compra>()
            .GetByIdAsync(dto.IdCompra, cancellationToken);

        if (compra == null)
            throw new NotFoundException("Compra", dto.IdCompra);

        // Validar que el código no esté duplicado (excepto el mismo lote)
        if (!string.IsNullOrEmpty(dto.Codigo))
        {
            var lotes = await _unitOfWork.Repository<Lote>().GetAllAsync(cancellationToken);
            if (lotes.Any(l => l.IdCompra == dto.IdCompra && l.Codigo == dto.Codigo && l.IdLote != request.Id))
            {
                throw new ValidationException($"Ya existe otro lote con el código {dto.Codigo} en esta compra");
            }
        }

        // Actualizar lote
        lote.IdCompra = dto.IdCompra;
        lote.Peso = dto.Peso;
        lote.Sacos = dto.Sacos;
        lote.Codigo = dto.Codigo;

        await _unitOfWork.Repository<Lote>().UpdateAsync(lote, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LoteDto>(lote);
    }
}
