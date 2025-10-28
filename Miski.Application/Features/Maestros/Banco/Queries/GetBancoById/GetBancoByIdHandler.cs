using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.Banco.Queries.GetBancoById;

public class GetBancoByIdHandler : IRequestHandler<GetBancoByIdQuery, BancoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBancoByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BancoDto> Handle(GetBancoByIdQuery request, CancellationToken cancellationToken)
    {
        var banco = await _unitOfWork.Repository<Domain.Entities.Banco>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (banco == null)
            throw new NotFoundException("Banco", request.Id);

        return _mapper.Map<BancoDto>(banco);
    }
}
