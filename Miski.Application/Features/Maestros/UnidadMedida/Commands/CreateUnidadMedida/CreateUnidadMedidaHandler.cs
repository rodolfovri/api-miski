using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.UnidadMedida.Commands.CreateUnidadMedida;

public class CreateUnidadMedidaHandler : IRequestHandler<CreateUnidadMedidaCommand, UnidadMedidaDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateUnidadMedidaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UnidadMedidaDto> Handle(CreateUnidadMedidaCommand request, CancellationToken cancellationToken)
    {
        // Verificar que no exista una unidad de medida con el mismo nombre o abreviatura
        var unidadesExistentes = await _unitOfWork.Repository<Domain.Entities.UnidadMedida>().GetAllAsync(cancellationToken);
        var existe = unidadesExistentes.Any(u => 
            u.Nombre.ToLower() == request.UnidadMedida.Nombre.ToLower() ||
            u.Abreviatura.ToLower() == request.UnidadMedida.Abreviatura.ToLower());

        if (existe)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                { "UnidadMedida", new[] { "Ya existe una unidad de medida con este nombre o abreviatura" } }
            });
        }

        // Crear la nueva unidad de medida
        var nuevaUnidad = new Domain.Entities.UnidadMedida
        {
            Nombre = request.UnidadMedida.Nombre,
            Abreviatura = request.UnidadMedida.Abreviatura
        };

        await _unitOfWork.Repository<Domain.Entities.UnidadMedida>().AddAsync(nuevaUnidad, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UnidadMedidaDto>(nuevaUnidad);
    }
}