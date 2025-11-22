using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Personas.CategoriaPersona.Commands.RevocarCategoria;

public class RevocarCategoriaHandler : IRequestHandler<RevocarCategoriaCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public RevocarCategoriaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(RevocarCategoriaCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Data;

        // Buscar la asignación
        var personaCategorias = await _unitOfWork.Repository<PersonaCategoria>()
            .GetAllAsync(cancellationToken);

        var asignacion = personaCategorias.FirstOrDefault(pc => 
            pc.IdPersona == dto.IdPersona && pc.IdCategoria == dto.IdCategoria);

        if (asignacion == null)
        {
            throw new NotFoundException("PersonaCategoria", $"Persona: {dto.IdPersona}, Categoría: {dto.IdCategoria}");
        }

        // Eliminar la asignación
        await _unitOfWork.Repository<PersonaCategoria>()
            .DeleteAsync(asignacion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
