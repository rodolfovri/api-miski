using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Vehiculo.Queries.GetVehiculos;

public class GetVehiculosHandler : IRequestHandler<GetVehiculosQuery, List<VehiculoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetVehiculosHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<VehiculoDto>> Handle(GetVehiculosQuery request, CancellationToken cancellationToken)
    {
        var vehiculos = await _unitOfWork.Repository<Domain.Entities.Vehiculo>()
            .GetAllAsync(cancellationToken);

        // Aplicar filtros
        if (!string.IsNullOrEmpty(request.Placa))
        {
            vehiculos = vehiculos.Where(v => v.Placa.Contains(request.Placa, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (!string.IsNullOrEmpty(request.Estado))
        {
            vehiculos = vehiculos.Where(v => v.Estado == request.Estado).ToList();
        }

        return vehiculos.Select(v => _mapper.Map<VehiculoDto>(v)).ToList();
    }
}
