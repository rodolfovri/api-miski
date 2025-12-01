using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.FAQ;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.FAQ.CategoriaFAQ.Commands.UpdateCategoria;

public class UpdateCategoriaHandler : IRequestHandler<UpdateCategoriaCommand, CategoriaFAQDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCategoriaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoriaFAQDto> Handle(UpdateCategoriaCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Categoria;

        var categoria = await _unitOfWork.Repository<Domain.Entities.CategoriaFAQ>()
            .GetByIdAsync(dto.IdCategoriaFAQ, cancellationToken);

        if (categoria == null)
            throw new NotFoundException("CategoriaFAQ", dto.IdCategoriaFAQ);

        categoria.Nombre = dto.Nombre;
        categoria.Estado = dto.Estado;

        await _unitOfWork.Repository<Domain.Entities.CategoriaFAQ>()
            .UpdateAsync(categoria, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoriaFAQDto>(categoria);
    }
}
