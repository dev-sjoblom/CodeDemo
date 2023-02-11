using CommunicationService.MetadataTypes.Models;

namespace CommunicationService.Classifications.DataModels;

public partial class Classification
{
    public Guid Id { get; private set; }

    public string Name { get; set; } = null!;
    public List<MetadataType> MetadataTypes { get; set; }
}