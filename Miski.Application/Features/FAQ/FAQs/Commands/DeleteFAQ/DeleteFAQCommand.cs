using MediatR;

namespace Miski.Application.Features.FAQ.FAQs.Commands.DeleteFAQ;

public record DeleteFAQCommand(int Id) : IRequest;
