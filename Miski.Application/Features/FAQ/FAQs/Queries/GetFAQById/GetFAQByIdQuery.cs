using MediatR;
using Miski.Shared.DTOs.FAQ;

namespace Miski.Application.Features.FAQ.FAQs.Queries.GetFAQById;

public record GetFAQByIdQuery(int Id) : IRequest<FAQDto>;
