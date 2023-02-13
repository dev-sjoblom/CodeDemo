using CommunicationService.MetadataTypes.Api.Model;
using CommunicationService.MetadataTypes.Data;
using CommunicationService.MetadataTypes.Fundamental;

namespace CommunicationService.MetadataTypes.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(MetadataTypeResponse), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class MetadataTypeGetByIdController : MetadataTypeBaseController
{
    private IMetadataTypeRepositoryReader MetadataTypeRepositoryReader { get; }

    public MetadataTypeGetByIdController(IMetadataTypeRepositoryReader metadataTypeRepositoryReader)
    {
        MetadataTypeRepositoryReader = metadataTypeRepositoryReader;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMetadataTypeById(Guid id, CancellationToken cancellationToken)
    {
        var metadataTypeResult = await MetadataTypeRepositoryReader.GetMetadataTypeById(id, cancellationToken);

        return metadataTypeResult.Match(
            item => Ok(item.ToMetadataTypeResponse()), 
            Problem);
    }
}