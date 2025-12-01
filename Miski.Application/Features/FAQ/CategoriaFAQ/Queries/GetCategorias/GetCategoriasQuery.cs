using MediatR;
using Miski.Shared.DTOs.FAQ;

namespace Miski.Application.Features.FAQ.CategoriaFAQ.Queries.GetCategorias;

public record GetCategoriasQuery(string? Estado = null) : IRequest<List<CategoriaFAQDto>>;
