using CommunicationService.MetadataTypes.DataAccess;

namespace CommunicationService.Classifications.DataAccess;

public class Classification
{
    public required Guid Id { get; init; }

    public required string Name { get; set; }
    public List<MetadataType> MetadataTypes { get; set; } = new();
}