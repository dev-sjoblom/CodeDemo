namespace CommunicationService.Classifications.Features.Upsert;

public class UpsertClassificationCommand : IRequest<ErrorOr<UpsertClassificationCommandResult>>
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string[] MetadataTypes { get; init; }
}