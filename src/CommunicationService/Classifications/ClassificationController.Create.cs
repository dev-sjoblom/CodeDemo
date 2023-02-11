using CommunicationService.Classifications.ContractModels;

namespace CommunicationService.Classifications;

public partial class ClassificationController
{
    [HttpPost]
    public async Task<IActionResult> CreateClassification(CreateClassificationRequest request,
        CancellationToken cancellationToken)
    {
        var toModelResult = request.ToClassification();
        if (toModelResult.IsError)
            return Problem(toModelResult.Errors);
        var classificationItem = toModelResult.Value;

        var createClassificationResult = await ClassificationRepository.CreateClassification(
            classificationItem,
            request.MetadataTypes,
            cancellationToken);

        return createClassificationResult.Match(
            _ => CreatedAtClassification(classificationItem),
            Problem);
    }
}