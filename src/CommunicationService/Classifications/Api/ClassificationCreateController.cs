using CommunicationService.Classifications.Api.Model;
using CommunicationService.Classifications.Core;
using CommunicationService.Classifications.Data;

namespace CommunicationService.Classifications.Api;
[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ClassificationResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class ClassificationCreateController : ClassificationBaseController
{
    private IClassificationRepositoryWriter RepositoryWriter { get; }

    public ClassificationCreateController(IClassificationRepositoryWriter repositoryWriter, ILogger<ClassificationCreateController> logger) : base(logger)
    {
        RepositoryWriter = repositoryWriter;
    }

    [HttpPost]
    public async Task<IActionResult> CreateClassification(CreateClassificationRequest request,
        CancellationToken cancellationToken)
    {
        var toModelResult = request.ToClassification();
        if (toModelResult.IsError)
            return Problem(toModelResult.Errors);
        var classificationItem = toModelResult.Value;

        var createClassificationResult = await RepositoryWriter.CreateClassification(
            classificationItem,
            request.MetadataTypes,
            cancellationToken);

        return createClassificationResult.Match(
            _ => CreatedAtClassification(classificationItem),
            Problem);
    }
}