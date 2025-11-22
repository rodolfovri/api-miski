using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Cargo.Commands.CreateCargo;

public class CreateCargoHandler : IRequestHandler<CreateCargoCommand, CargoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCargoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CargoDto> Handle(CreateCargoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Cargo;

        var cargo = new Domain.Entities.Cargo
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Estado = dto.Estado ?? "ACTIVO",
            FRegistro = DateTime.UtcNow
        };

        await _unitOfWork.Repository<Domain.Entities.Cargo>()
            .AddAsync(cargo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CargoDto>(cargo);
    }
}
