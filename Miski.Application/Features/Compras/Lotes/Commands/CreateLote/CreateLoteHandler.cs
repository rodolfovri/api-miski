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

        // Validar que el código no esté duplicado si se proporciona
        if (!string.IsNullOrEmpty(dto.Codigo))
        {
            var lotes = await _unitOfWork.Repository<Lote>().GetAllAsync(cancellationToken);
            if (lotes.Any(l => l.Codigo == dto.Codigo))
            {
                throw new ValidationException($"Ya existe un lote con el código {dto.Codigo}");
            }
        }

        // ? Crear el lote SIN asignarlo a una compra (relación 1:1)
        // La asignación se hará posteriormente con AsignarLoteACompra
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

        return _mapper.Map<LoteDto>(lote);
    }
}
