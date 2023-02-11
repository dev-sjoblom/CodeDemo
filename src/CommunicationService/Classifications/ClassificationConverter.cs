using CommunicationService.Classifications.ContractModels;
using CommunicationService.Classifications.DataModels;

namespace CommunicationService.Classifications;

public static class ClassificationConverter
{
    public static ErrorOr<Classification> ToClassification(this UpsertClassificationRequest request, Guid id)
    {
        return Classification.Create(request.Name, id: id);
    }

    public static ErrorOr<Classification> ToClassification(this CreateClassificationRequest request)
    {
        return Classification.Create(request.Name);
    }
    
    public static ClassificationResponse ToClassificationResponse(this Classification classification)
    {
        return new ClassificationResponse(
            classification.Id,
            classification.Name,
            classification.MetadataTypes.Select(x => x.Name).ToArray());
    }
}