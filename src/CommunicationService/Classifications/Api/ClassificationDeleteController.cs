using CommunicationService.Classifications.Data;

namespace CommunicationService.Classifications.Api;

[ApiController]
[ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public  class ClassificationDeleteController : ClassificationBaseController
{
    private IClassificationRepositoryWriter RepositoryWriter { get; }

    public ClassificationDeleteController(IClassificationRepositoryWriter repositoryWriter)
    {
        RepositoryWriter = repositoryWriter;
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteClassification(Guid id, CancellationToken cancellationToken)
    {
        var deleteResult = await
            RepositoryWriter.DeleteClassification(id, cancellationToken);

        return deleteResult.Match(_ => NoContent(), Problem);
    }
}