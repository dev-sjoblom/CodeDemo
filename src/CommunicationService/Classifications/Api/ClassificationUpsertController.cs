using CommunicationService.Classifications.Api.Model;
using CommunicationService.Classifications.Core;
using CommunicationService.Classifications.Data;

namespace CommunicationService.Classifications.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ClassificationResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
[Route("[controller]")]
public class ClassificationUpsertController : ClassificationBaseController
{
    private IClassificationRepositoryWriter RepositoryWriter { get; }

    public ClassificationUpsertController(IClassificationRepositoryWriter repositoryWriter, ILogger<ClassificationUpsertController> logger) : base(logger)
    {
        RepositoryWriter = repositoryWriter;
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
        var upsertedResult = await RepositoryWriter.UpsertClassification(
            classification,
            request.MetadataTypes,
            cancellationToken);

        return upsertedResult.Match(item => item.RegisteredAsNewItem
                ? CreatedAtClassification(classification)
                : NoContent(),
            Problem);
    }
}