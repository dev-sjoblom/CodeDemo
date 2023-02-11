namespace CommunicationService.Classifications;

public partial class ClassificationController
{
    [HttpGet("ById/{id:guid}")]
    public async Task<IActionResult> GetClassificationById(Guid id, CancellationToken cancellationToken)
    {
        var classificationResult = await ClassificationRepository.GetClassificationById(id, cancellationToken);

        return classificationResult.Match(
            item => Ok(item.ToClassificationResponse()),
            Problem);
    }
}