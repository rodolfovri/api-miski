using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.FAQ;

namespace Miski.Application.Features.FAQ.CategoriaFAQ.Commands.CreateCategoria;

public class CreateCategoriaHandler : IRequestHandler<CreateCategoriaCommand, CategoriaFAQDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCategoriaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoriaFAQDto> Handle(CreateCategoriaCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Categoria;

        var categoria = new Domain.Entities.CategoriaFAQ
        {
            Nombre = dto.Nombre,
            Estado = dto.Estado
        };

        var categoriaCreada = await _unitOfWork.Repository<Domain.Entities.CategoriaFAQ>()
            .AddAsync(categoria, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoriaFAQDto>(categoriaCreada);
    }
}
