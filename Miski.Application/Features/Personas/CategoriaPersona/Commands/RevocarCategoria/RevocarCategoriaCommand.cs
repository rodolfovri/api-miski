using MediatR;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.CategoriaPersona.Commands.RevocarCategoria;

public record RevocarCategoriaCommand(RevocarCategoriaDto Data) : IRequest<Unit>;
