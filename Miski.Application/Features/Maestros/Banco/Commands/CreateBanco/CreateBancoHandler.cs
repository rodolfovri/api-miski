using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Banco.Commands.CreateBanco;

public class CreateBancoHandler : IRequestHandler<CreateBancoCommand, BancoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBancoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BancoDto> Handle(CreateBancoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Banco;

        var banco = new Domain.Entities.Banco
        {
            Nombre = dto.Nombre,
            Estado = dto.Estado ?? "ACTIVO"
        };

        await _unitOfWork.Repository<Domain.Entities.Banco>()
            .AddAsync(banco, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<BancoDto>(banco);
    }
}
