using MediatR;

namespace Miski.Application.Features.FAQ.CategoriaFAQ.Commands.DeleteCategoria;

public record DeleteCategoriaCommand(int Id) : IRequest;
