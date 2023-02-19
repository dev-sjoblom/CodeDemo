using CommunicationService.MetadataTypes.Data;

namespace CommunicationService.MetadataTypes.Commands;

public class UpsertMetadataTypeCommandResult
{
    public required bool RegisteredAsNewItem { get; init; }
    public required MetadataType MetadataType { get; init; }
}