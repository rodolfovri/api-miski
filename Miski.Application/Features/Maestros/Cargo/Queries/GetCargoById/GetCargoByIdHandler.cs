using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.Cargo.Queries.GetCargoById;

public class GetCargoByIdHandler : IRequestHandler<GetCargoByIdQuery, CargoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCargoByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CargoDto> Handle(GetCargoByIdQuery request, CancellationToken cancellationToken)
    {
        var cargo = await _unitOfWork.Repository<Domain.Entities.Cargo>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (cargo == null)
        {
            throw new NotFoundException("Cargo", request.Id);
        }

        return _mapper.Map<CargoDto>(cargo);
    }
}
