using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.UnidadMedida.Queries.GetUnidadMedidas;

public class GetUnidadMedidasHandler : IRequestHandler<GetUnidadMedidasQuery, List<UnidadMedidaDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUnidadMedidasHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<UnidadMedidaDto>> Handle(GetUnidadMedidasQuery request, CancellationToken cancellationToken)
    {
        var unidadesMedida = await _unitOfWork.Repository<Domain.Entities.UnidadMedida>().GetAllAsync(cancellationToken);

        // Aplicar filtro por nombre si se proporciona
        if (!string.IsNullOrEmpty(request.Nombre))
        {
            unidadesMedida = unidadesMedida.Where(u =>
                u.Nombre.Contains(request.Nombre, StringComparison.OrdinalIgnoreCase) ||
                u.Abreviatura.Contains(request.Nombre, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return unidadesMedida.Select(u => _mapper.Map<UnidadMedidaDto>(u)).ToList();
    }
}