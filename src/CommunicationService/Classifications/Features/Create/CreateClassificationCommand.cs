using CommunicationService.Classifications.DataAccess;

namespace CommunicationService.Classifications.Features.Create;

public class CreateClassificationCommand : IRequest<ErrorOr<Classification>>
{
    public required string Name { get; init; }
    public required string[] MetadataTypes { get; init; }
}