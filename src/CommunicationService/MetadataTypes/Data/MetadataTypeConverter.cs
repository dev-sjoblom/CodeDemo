using CommunicationService.MetadataTypes.Api.Model;

namespace CommunicationService.MetadataTypes.Data;

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