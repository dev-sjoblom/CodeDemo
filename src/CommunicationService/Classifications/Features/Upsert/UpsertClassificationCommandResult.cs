using CommunicationService.Classifications.DataAccess;

namespace CommunicationService.Classifications.Features.Upsert;

public class UpsertClassificationCommandResult
{
    public required bool RegisteredAsNewItem { get; init; }
    public required Classification Classification { get; init; }
}