using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.UnidadMedida.Commands.UpdateUnidadMedida;

public class UpdateUnidadMedidaHandler : IRequestHandler<UpdateUnidadMedidaCommand, UnidadMedidaDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUnidadMedidaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UnidadMedidaDto> Handle(UpdateUnidadMedidaCommand request, CancellationToken cancellationToken)
    {
        // Buscar la unidad de medida
        var unidadMedida = await _unitOfWork.Repository<Domain.Entities.UnidadMedida>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (unidadMedida == null)
            throw new NotFoundException("UnidadMedida", request.Id);

        // Verificar que no exista otra unidad con el mismo nombre o abreviatura
        var unidadesExistentes = await _unitOfWork.Repository<Domain.Entities.UnidadMedida>().GetAllAsync(cancellationToken);
        var existe = unidadesExistentes.Any(u =>
            (u.Nombre.ToLower() == request.UnidadMedida.Nombre.ToLower() ||
             u.Abreviatura.ToLower() == request.UnidadMedida.Abreviatura.ToLower()) &&
            u.IdUnidadMedida != request.Id);

        if (existe)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                { "UnidadMedida", new[] { "Ya existe otra unidad de medida con este nombre o abreviatura" } }
            });
        }

        // Actualizar la unidad de medida
        unidadMedida.Nombre = request.UnidadMedida.Nombre;
        unidadMedida.Abreviatura = request.UnidadMedida.Abreviatura;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UnidadMedidaDto>(unidadMedida);
    }
}