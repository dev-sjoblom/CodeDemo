using MediatR;

namespace CommunicationService.MetadataTypes.Features.Delete;

public class DeleteMetadataTypeCommand : IRequest<ErrorOr<Deleted>>
{
    public required Guid Id { get; init; }
}