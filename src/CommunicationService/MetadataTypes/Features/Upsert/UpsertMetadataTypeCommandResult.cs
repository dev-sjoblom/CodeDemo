using CommunicationService.MetadataTypes.DataAccess;

namespace CommunicationService.MetadataTypes.Features.Upsert;

public class UpsertMetadataTypeCommandResult
{
    public required bool RegisteredAsNewItem { get; init; }
    public required MetadataType MetadataType { get; init; }
}