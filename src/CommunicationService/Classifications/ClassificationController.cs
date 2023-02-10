using CommunicationService.Classifications.ContractModels;
using CommunicationService.Classifications.DataModels;

namespace CommunicationService.Classifications;

[ApiController]
[Route("[controller]")]
public class ClassificationController : ApiController
{
    private IClassificationRepository ClassificationRepository { get; }

    public ClassificationController(IClassificationRepository classificationRepository)
    {
        ClassificationRepository = classificationRepository;
    }

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

    [HttpGet]
    public async Task<IActionResult> ListClassifications(CancellationToken cancellationToken)
    {
        var classificationsResult = await ClassificationRepository.ListClassifications(cancellationToken);

        return classificationsResult.Match(
            item => Ok(item.Select(x => x.ToClassificationResponse())),
            Problem);
    }

    [HttpGet("ById/{id:guid}")]
    public async Task<IActionResult> GetClassificationById(Guid id, CancellationToken cancellationToken)
    {
        var classificationResult = await ClassificationRepository.GetClassificationById(id, cancellationToken);

        return classificationResult.Match(
            item => Ok(item.ToClassificationResponse()),
            Problem);
    }

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

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteClassification(Guid id, CancellationToken cancellationToken)
    {
        var deleteResult = await
            ClassificationRepository.DeleteClassification(id, cancellationToken);

        return deleteResult.Match(_ => NoContent(), Problem);
    }

    private CreatedAtActionResult CreatedAtClassification(Classification classification)
    {
        return CreatedAtAction(
            actionName: nameof(GetClassificationById),
            routeValues: new { id = classification.Id },
            value: classification.ToClassificationResponse());
    }
}