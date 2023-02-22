using CommunicationService.Classifications.DataStore;

namespace CommunicationService.MetadataTypes.DataStore;

public class MetadataTypeClassification
{
    public required Guid MetadataTypeId { get; set; }
    public required Guid ClassificationId { get; set; }
    public required MetadataType MetadataType { get; set; }
    public required Classification Classification { get; set; }
}