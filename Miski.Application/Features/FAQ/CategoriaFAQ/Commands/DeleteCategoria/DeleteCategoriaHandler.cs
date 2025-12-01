using MediatR;
using Miski.Domain.Contracts;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.FAQ.CategoriaFAQ.Commands.DeleteCategoria;

public class DeleteCategoriaHandler : IRequestHandler<DeleteCategoriaCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoriaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoria = await _unitOfWork.Repository<Domain.Entities.CategoriaFAQ>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (categoria == null)
            throw new NotFoundException("CategoriaFAQ", request.Id);

        // Cambiar estado a INACTIVO (eliminación lógica)
        categoria.Estado = "INACTIVO";

        await _unitOfWork.Repository<Domain.Entities.CategoriaFAQ>()
            .UpdateAsync(categoria, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
