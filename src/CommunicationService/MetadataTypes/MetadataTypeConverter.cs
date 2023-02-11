using CommunicationService.MetadataTypes.Contracts;
using CommunicationService.MetadataTypes.Models;

namespace CommunicationService.MetadataTypes;

public static class MetadataTypeConverter
{
    public static ErrorOr<MetadataType> ToMetadataType(this UpsertMetadataTypeRequest request, Guid id)
    {
        return MetadataType.Create(request.Name, id: id);
    }

    public static ErrorOr<MetadataType> ToMetadataType(this CreateMetadataTypeRequest request)
    {
        return MetadataType.Create(request.Name);
    }
    
    public static MetadataTypeResponse  ToMetadataTypeResponse(this MetadataType metadataType)
    {
        return new MetadataTypeResponse(
            metadataType.Id,
            metadataType.Name,
            metadataType.Classifications.Select(x => x.Name).ToArray());
    }
}