using MediatR;
using Miski.Shared.DTOs.FAQ;

namespace Miski.Application.Features.FAQ.CategoriaFAQ.Commands.UpdateCategoria;

public record UpdateCategoriaCommand(int Id, UpdateCategoriaFAQDto Categoria) : IRequest<CategoriaFAQDto>;
