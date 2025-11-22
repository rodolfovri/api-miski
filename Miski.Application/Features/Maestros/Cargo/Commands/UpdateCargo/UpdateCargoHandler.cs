using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.Cargo.Commands.UpdateCargo;

public class UpdateCargoHandler : IRequestHandler<UpdateCargoCommand, CargoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCargoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CargoDto> Handle(UpdateCargoCommand request, CancellationToken cancellationToken)
    {
        var cargo = await _unitOfWork.Repository<Domain.Entities.Cargo>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (cargo == null)
        {
            throw new NotFoundException("Cargo", request.Id);
        }

        var dto = request.Cargo;

        cargo.Nombre = dto.Nombre;
        cargo.Descripcion = dto.Descripcion;
        
        if (!string.IsNullOrEmpty(dto.Estado))
        {
            cargo.Estado = dto.Estado;
        }

        await _unitOfWork.Repository<Domain.Entities.Cargo>()
            .UpdateAsync(cargo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CargoDto>(cargo);
    }
}
