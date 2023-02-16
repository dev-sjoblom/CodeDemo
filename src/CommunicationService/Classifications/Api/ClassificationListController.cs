using CommunicationService.Classifications.Api.Model;
using CommunicationService.Classifications.Core;
using CommunicationService.Classifications.Data;

namespace CommunicationService.Classifications.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ClassificationResponse[]), StatusCodes.Status200OK)]
[Route("[controller]")]
public class ClassificationListController : ClassificationBaseController
{
    private IClassificationRepositoryReader ClassificationRepositoryReader { get; }

    public ClassificationListController(IClassificationRepositoryReader classificationRepositoryReader, ILogger<ClassificationListController> logger) : base(logger)
    {
        ClassificationRepositoryReader = classificationRepositoryReader;
    }

    [HttpGet]
    public async Task<IActionResult> ListClassifications(CancellationToken cancellationToken)
    {
        var classificationsResult = await ClassificationRepositoryReader.ListClassifications(cancellationToken);

        return classificationsResult.Match(
            item => Ok(item.Select(x => x.ToClassificationResponse())),
            Problem);
    }
}