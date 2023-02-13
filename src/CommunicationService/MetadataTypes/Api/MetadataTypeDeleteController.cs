using CommunicationService.MetadataTypes.Data;

namespace CommunicationService.MetadataTypes.Api;

[ApiController]
[ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class MetadataTypeDeleteController : MetadataTypeBaseController
{
    private IMetadataTypeRepositoryWriter RepositoryWriter { get; }

    public MetadataTypeDeleteController(IMetadataTypeRepositoryWriter repositoryWriter)
    {
        RepositoryWriter = repositoryWriter;
    }

    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBreakfast(Guid id, CancellationToken cancellationToken)
    {
        var deleteResult = await RepositoryWriter.DeleteMetadataType(id, cancellationToken);
    
        return deleteResult.Match(_ => NoContent(), Problem);
    }
}