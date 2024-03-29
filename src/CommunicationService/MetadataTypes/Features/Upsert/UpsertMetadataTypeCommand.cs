namespace CommunicationService.MetadataTypes.Features.Upsert;

public class UpsertMetadataTypeCommand : IRequest<ErrorOr<UpsertMetadataTypeCommandResult>>
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string[] Classifications { get; init; }
}