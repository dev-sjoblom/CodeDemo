using CommunicationService.Classifications.Api.Model;
using CommunicationService.Classifications.Core;
using CommunicationService.Classifications.Data;

namespace CommunicationService.Classifications.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ClassificationResponse), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class ClassificationGetByIdController : ClassificationBaseController
{
    private IClassificationRepositoryReader ClassificationRepositoryReader { get; }

    public ClassificationGetByIdController(IClassificationRepositoryReader classificationRepositoryReader, ILogger<ClassificationGetByIdController> logger) : base(logger)
    {
        ClassificationRepositoryReader = classificationRepositoryReader;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetClassificationById(Guid id, CancellationToken cancellationToken)
    {
        var classificationResult = await ClassificationRepositoryReader.GetClassificationById(id, cancellationToken);

        return classificationResult.Match(
            item => Ok(item.ToClassificationResponse()),
            Problem);
    }
}