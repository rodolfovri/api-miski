using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.UnidadMedida.Queries.GetUnidadMedidaById;

public class GetUnidadMedidaByIdHandler : IRequestHandler<GetUnidadMedidaByIdQuery, UnidadMedidaDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUnidadMedidaByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UnidadMedidaDto> Handle(GetUnidadMedidaByIdQuery request, CancellationToken cancellationToken)
    {
        var unidadMedida = await _unitOfWork.Repository<Domain.Entities.UnidadMedida>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (unidadMedida == null)
            throw new NotFoundException("UnidadMedida", request.Id);

        return _mapper.Map<UnidadMedidaDto>(unidadMedida);
    }
}