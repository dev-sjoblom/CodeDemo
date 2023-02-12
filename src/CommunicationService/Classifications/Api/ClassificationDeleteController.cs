using CommunicationService.Classifications.Fundamental;

namespace CommunicationService.Classifications.Api;

[ApiController]
[Route("[controller]")]
public  class ClassificationDeleteController : ClassificationBaseController
{
    private IClassificationRepository ClassificationRepository { get; }

    public ClassificationDeleteController(IClassificationRepository classificationRepository)
    {
        ClassificationRepository = classificationRepository;
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteClassification(Guid id, CancellationToken cancellationToken)
    {
        var deleteResult = await
            ClassificationRepository.DeleteClassification(id, cancellationToken);

        return deleteResult.Match(_ => NoContent(), Problem);
    }
}