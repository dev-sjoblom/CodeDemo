using CommunicationService.Classifications.DataModels;

namespace CommunicationService.MetadataTypes.DataModels;

public class MetadataTypeClassification
{
    public Guid MetadataTypeId { get; set; }
    public Guid ClassificationId { get; set; }
    public MetadataType MetadataType { get; set; } = null!;
    public Classification Classification { get; set; } = null!;
}