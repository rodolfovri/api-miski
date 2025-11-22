using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.Rol.Commands.UpdateRol;

public class UpdateRolHandler : IRequestHandler<UpdateRolCommand, RolMaestroDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateRolHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<RolMaestroDto> Handle(UpdateRolCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Rol;

        var rol = await _unitOfWork.Repository<Domain.Entities.Rol>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (rol == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Rol), request.Id);
        }

        rol.Nombre = dto.Nombre;
        rol.Descripcion = dto.Descripcion;
        rol.TipoPlataforma = dto.TipoPlataforma;
        
        if (!string.IsNullOrEmpty(dto.Estado))
        {
            rol.Estado = dto.Estado;
        }

        await _unitOfWork.Repository<Domain.Entities.Rol>()
            .UpdateAsync(rol, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RolMaestroDto>(rol);
    }
}
