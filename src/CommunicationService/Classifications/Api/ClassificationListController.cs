using CommunicationService.Classifications.Api.Model;
using CommunicationService.Classifications.Data;
using CommunicationService.Classifications.Fundamental;

namespace CommunicationService.Classifications.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ClassificationResponse[]), StatusCodes.Status200OK)]
[Route("[controller]")]
public class ClassificationListController : ClassificationBaseController
{
    private IClassificationRepository ClassificationRepository { get; }

    public ClassificationListController(IClassificationRepository classificationRepository)
    {
        ClassificationRepository = classificationRepository;
    }

    [HttpGet]
    public async Task<IActionResult> ListClassifications(CancellationToken cancellationToken)
    {
        var classificationsResult = await ClassificationRepository.ListClassifications(cancellationToken);

        return classificationsResult.Match(
            item => Ok(item.Select(x => x.ToClassificationResponse())),
            Problem);
    }
}