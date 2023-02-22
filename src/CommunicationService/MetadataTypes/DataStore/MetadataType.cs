using CommunicationService.Classifications.DataStore;

namespace CommunicationService.MetadataTypes.DataStore;

public class MetadataType
{
    public required Guid Id { get; init; }

    public required string Name { get; set; }

    public List<Classification> Classifications { get; } = new();
}