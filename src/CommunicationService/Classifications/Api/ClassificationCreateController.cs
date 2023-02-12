using CommunicationService.Classifications.Api.Model;
using CommunicationService.Classifications.Data;
using CommunicationService.Classifications.Fundamental;

namespace CommunicationService.Classifications.Api;
[ApiController]
[Route("[controller]")]
public class ClassificationCreateController : ClassificationBaseController
{
    private IClassificationRepository ClassificationRepository { get; }

    public ClassificationCreateController(IClassificationRepository classificationRepository)
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
}