using CommunicationService.MetadataTypes.DataStore;

namespace CommunicationService.Classifications.DataStore;

public class Classification
{
    public required Guid Id { get; init; }

    public required string Name { get; set; }
    public List<MetadataType> MetadataTypes { get; set; } = new();
}