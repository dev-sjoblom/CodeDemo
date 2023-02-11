using CommunicationService.Classifications.DataModels;

namespace CommunicationService.Classifications;

[ApiController]
[Route("[controller]")]
public partial class ClassificationController : ApiController
{
    private IClassificationRepository ClassificationRepository { get; }

    public ClassificationController(IClassificationRepository classificationRepository)
    {
        ClassificationRepository = classificationRepository;
    }
    
    private CreatedAtActionResult CreatedAtClassification(Classification classification)
    {
        return CreatedAtAction(
            actionName: nameof(GetClassificationById),
            routeValues: new { id = classification.Id },
            value: classification.ToClassificationResponse());
    }
}