using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.TipoDocumento.Commands.CreateTipoDocumento;

public class CreateTipoDocumentoHandler : IRequestHandler<CreateTipoDocumentoCommand, TipoDocumentoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTipoDocumentoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TipoDocumentoDto> Handle(CreateTipoDocumentoCommand request, CancellationToken cancellationToken)
    {
        // Verificar que no exista otro tipo con el mismo nombre
        var tiposExistentes = await _unitOfWork.Repository<Domain.Entities.TipoDocumento>().GetAllAsync(cancellationToken);
        var existe = tiposExistentes.Any(t => t.Nombre.ToLower() == request.TipoDocumento.Nombre.ToLower());
        
        if (existe)
        {
            throw new ValidationException("Ya existe un tipo de documento con este nombre");
        }

        var nuevoTipo = new Domain.Entities.TipoDocumento
        {
            Nombre = request.TipoDocumento.Nombre,
            LongitudMin = request.TipoDocumento.LongitudMin,
            LongitudMax = request.TipoDocumento.LongitudMax
        };

        await _unitOfWork.Repository<Domain.Entities.TipoDocumento>().AddAsync(nuevoTipo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TipoDocumentoDto>(nuevoTipo);
    }
}