using CommunicationService.MetadataTypes.DataAccess;

namespace CommunicationService.MetadataTypes.Features.Create;

public class CreateMetadataTypeCommand : IRequest<ErrorOr<MetadataType>>
{
    public required string Name { get; init; }
    public required string[] Classifications { get; init; }
}