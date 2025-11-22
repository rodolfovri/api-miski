using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Rol.Commands.CreateRol;

public class CreateRolHandler : IRequestHandler<CreateRolCommand, RolMaestroDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateRolHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<RolMaestroDto> Handle(CreateRolCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Rol;

        var rol = new Domain.Entities.Rol
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            TipoPlataforma = dto.TipoPlataforma,
            Estado = dto.Estado ?? "ACTIVO"
        };

        await _unitOfWork.Repository<Domain.Entities.Rol>()
            .AddAsync(rol, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RolMaestroDto>(rol);
    }
}
