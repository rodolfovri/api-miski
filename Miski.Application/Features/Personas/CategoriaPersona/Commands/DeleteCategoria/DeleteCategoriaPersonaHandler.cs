using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Personas.CategoriaPersona.Commands.DeleteCategoria;

public class DeleteCategoriaPersonaHandler : IRequestHandler<DeleteCategoriaPersonaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoriaPersonaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteCategoriaPersonaCommand request, CancellationToken cancellationToken)
    {
        var categoria = await _unitOfWork.Repository<Domain.Entities.CategoriaPersona>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (categoria == null)
            throw new NotFoundException("CategoriaPersona", request.Id);

        // Verificar si hay personas asociadas a esta categoría
        var personaCategorias = await _unitOfWork.Repository<PersonaCategoria>().GetAllAsync(cancellationToken);
        var tienePersonas = personaCategorias.Any(pc => pc.IdCategoria == request.Id);

        if (tienePersonas)
        {
            throw new ValidationException("No se puede eliminar la categoría porque tiene personas asociadas");
        }

        await _unitOfWork.Repository<Domain.Entities.CategoriaPersona>().DeleteAsync(categoria, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}