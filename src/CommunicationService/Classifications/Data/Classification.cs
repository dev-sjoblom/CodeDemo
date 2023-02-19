using CommunicationService.MetadataTypes.Data;

namespace CommunicationService.Classifications.Data;

public partial class Classification
{
    public Guid Id { get; }

    public string Name { get; set; } = null!;
    public List<MetadataType> MetadataTypes { get; set; }
}