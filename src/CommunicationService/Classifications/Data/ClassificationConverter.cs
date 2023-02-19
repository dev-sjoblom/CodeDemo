using CommunicationService.Classifications.Api.Model;

namespace CommunicationService.Classifications.Data;

public static class ClassificationConverter
{
    public static ClassificationResponse ToClassificationResponse(this Classification classification)
    {
        return new ClassificationResponse(
            classification.Id,
            classification.Name,
            classification.MetadataTypes.Select(x => x.Name).ToArray());
    }
}