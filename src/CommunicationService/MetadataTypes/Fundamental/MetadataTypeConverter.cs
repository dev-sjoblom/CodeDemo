using CommunicationService.MetadataTypes.DataStore;
using CommunicationService.MetadataTypes.Features;

namespace CommunicationService.MetadataTypes.Fundamental;

public static class MetadataTypeConverter
{
    public static MetadataTypeResponse ToMetadataTypeResponse(this MetadataType metadataType)
    {
        return new MetadataTypeResponse(
            metadataType.Id,
            metadataType.Name,
            metadataType.Classifications.Select(x => x.Name).ToArray());
    }
}