using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Permisos;

namespace Miski.Application.Features.Permisos.Queries.GetAcciones;

public class GetAccionesHandler : IRequestHandler<GetAccionesQuery, List<AccionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAccionesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<AccionDto>> Handle(GetAccionesQuery request, CancellationToken cancellationToken)
    {
        var acciones = await _unitOfWork.Repository<Accion>().GetAllAsync(cancellationToken);

        var accionesActivas = acciones
            .Where(a => a.Estado == "ACTIVO")
            .OrderBy(a => a.Orden)
            .ToList();

        return _mapper.Map<List<AccionDto>>(accionesActivas);
    }
}
