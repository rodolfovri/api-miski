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

        // ? Crear el lote SIN asignarlo a una compra (relación 1:1)
        // La asignación se hará posteriormente con AsignarLoteACompra
        var lote = new Lote
        {
            Peso = dto.Peso,
            Sacos = dto.Sacos,
            Codigo = null, // Se generará automáticamente después de obtener el IdLote
            Comision = dto.Comision,
            Observacion = dto.Observacion
        };

        await _unitOfWork.Repository<Lote>().AddAsync(lote, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Generar el código del lote usando el IdLote generado
        var anioActual = DateTime.UtcNow.Year;
        lote.Codigo = $"LOTE-MP-{anioActual}-{lote.IdLote:D8}";

        await _unitOfWork.Repository<Lote>().UpdateAsync(lote, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LoteDto>(lote);
    }
}
