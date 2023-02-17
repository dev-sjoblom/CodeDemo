using MediatR;

namespace CommunicationService.MetadataTypes.Commands;

public class DeleteMetadataTypeCommand : IRequest<ErrorOr<Deleted>>
{
    public required Guid Id { get; init; }
}