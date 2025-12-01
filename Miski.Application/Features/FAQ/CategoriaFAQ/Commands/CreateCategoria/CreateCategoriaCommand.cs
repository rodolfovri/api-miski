using MediatR;
using Miski.Shared.DTOs.FAQ;

namespace Miski.Application.Features.FAQ.CategoriaFAQ.Commands.CreateCategoria;

public record CreateCategoriaCommand(CreateCategoriaFAQDto Categoria) : IRequest<CategoriaFAQDto>;
