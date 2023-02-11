using CommunicationService.Classifications.ContractModels;

namespace CommunicationService.Classifications;

public partial class ClassificationController
{
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpsertClassification(Guid id,
        UpsertClassificationRequest request,
        CancellationToken cancellationToken)
    {
        var classificationResult = request.ToClassification(id);

        if (classificationResult.IsError)
        {
            return Problem(classificationResult.Errors);
        }

        var classification = classificationResult.Value;
        var upsertedResult = await ClassificationRepository.UpsertClassification(
            classification,
            request.MetadataTypes,
            cancellationToken);

        return upsertedResult.Match(item => item.RegisteredAsNewItem
                ? CreatedAtClassification(classification)
                : NoContent(),
            Problem);
    }

}