using MediatR;

namespace CommunicationService.Classifications.Features.Delete;

public class DeleteClassificationCommand : IRequest<ErrorOr<Deleted>>
{
    public required Guid Id { get; init; }
}