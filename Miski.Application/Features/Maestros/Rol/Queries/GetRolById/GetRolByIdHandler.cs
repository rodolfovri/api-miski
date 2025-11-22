using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.Rol.Queries.GetRolById;

public class GetRolByIdHandler : IRequestHandler<GetRolByIdQuery, RolMaestroDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRolByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<RolMaestroDto> Handle(GetRolByIdQuery request, CancellationToken cancellationToken)
    {
        var rol = await _unitOfWork.Repository<Domain.Entities.Rol>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (rol == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Rol), request.Id);
        }

        return _mapper.Map<RolMaestroDto>(rol);
    }
}
