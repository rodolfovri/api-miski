using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.Banco.Commands.UpdateBanco;

public class UpdateBancoHandler : IRequestHandler<UpdateBancoCommand, BancoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBancoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BancoDto> Handle(UpdateBancoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Banco;

        var banco = await _unitOfWork.Repository<Domain.Entities.Banco>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (banco == null)
            throw new NotFoundException("Banco", request.Id);

        // Actualizar
        banco.Nombre = dto.Nombre;
        banco.Estado = dto.Estado ?? banco.Estado;

        await _unitOfWork.Repository<Domain.Entities.Banco>()
            .UpdateAsync(banco, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<BancoDto>(banco);
    }
}
