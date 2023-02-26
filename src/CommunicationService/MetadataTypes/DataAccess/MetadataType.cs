using CommunicationService.Classifications.DataAccess;

namespace CommunicationService.MetadataTypes.DataAccess;

public class MetadataType
{
    public required Guid Id { get; init; }

    public required string Name { get; set; }

    public List<Classification> Classifications { get; } = new();
}