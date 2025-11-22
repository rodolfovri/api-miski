using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Rol.Queries.GetRoles;

public class GetRolesHandler : IRequestHandler<GetRolesQuery, List<RolMaestroDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRolesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<RolMaestroDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _unitOfWork.Repository<Domain.Entities.Rol>()
            .GetAllAsync(cancellationToken);

        // Aplicar filtro de tipo de plataforma si se proporciona
        if (!string.IsNullOrEmpty(request.TipoPlataforma))
        {
            roles = roles.Where(r => r.TipoPlataforma == request.TipoPlataforma).ToList();
        }

        // Aplicar filtro de estado si se proporciona
        if (!string.IsNullOrEmpty(request.Estado))
        {
            roles = roles.Where(r => r.Estado == request.Estado).ToList();
        }

        return roles.Select(r => _mapper.Map<RolMaestroDto>(r)).ToList();
    }
}
