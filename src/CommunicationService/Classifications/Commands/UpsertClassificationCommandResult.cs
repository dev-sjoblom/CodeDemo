using CommunicationService.Classifications.Data;

namespace CommunicationService.Classifications.Commands;

public class UpsertClassificationCommandResult
{
    public required bool RegisteredAsNewItem { get; init; }
    public required Classification Classification { get; init; }
}