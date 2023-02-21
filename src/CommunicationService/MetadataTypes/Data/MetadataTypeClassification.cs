using CommunicationService.Classifications.Data;

namespace CommunicationService.MetadataTypes.Data;

public class MetadataTypeClassification
{
    public required Guid MetadataTypeId { get; set; }
    public required Guid ClassificationId { get; set; }
    public required MetadataType MetadataType { get; set; }
    public required Classification Classification { get; set; }
}