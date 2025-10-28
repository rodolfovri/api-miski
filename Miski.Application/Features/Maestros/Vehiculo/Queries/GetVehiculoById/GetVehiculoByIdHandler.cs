using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.Vehiculo.Queries.GetVehiculoById;

public class GetVehiculoByIdHandler : IRequestHandler<GetVehiculoByIdQuery, VehiculoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetVehiculoByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<VehiculoDto> Handle(GetVehiculoByIdQuery request, CancellationToken cancellationToken)
    {
        var vehiculo = await _unitOfWork.Repository<Domain.Entities.Vehiculo>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (vehiculo == null)
            throw new NotFoundException("Vehiculo", request.Id);

        return _mapper.Map<VehiculoDto>(vehiculo);
    }
}
