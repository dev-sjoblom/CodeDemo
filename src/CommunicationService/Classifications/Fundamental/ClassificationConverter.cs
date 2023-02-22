using CommunicationService.Classifications.DataStore;
using CommunicationService.Classifications.Features;

namespace CommunicationService.Classifications.Fundamental;

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