using CommunicationService.MetadataTypes.Data;
using CommunicationService.MetadataTypes.Fundamental;

namespace CommunicationService.MetadataTypes.Api;

[ApiController]
[Produces("application/json")]
[Route("[controller]")]
public class MetadataTypeGetByNameController : MetadataTypeBaseController
{
    private IMetadataTypeRepositoryReader MetadataTypeRepositoryReader { get; }

    public MetadataTypeGetByNameController(IMetadataTypeRepositoryReader metadataTypeRepositoryReader)
    {
        MetadataTypeRepositoryReader = metadataTypeRepositoryReader;
    }

    
    [HttpGet("{name}")]
    public async Task<IActionResult> GetMetadataTypeByName(string name, CancellationToken cancellationToken)
    {
        var metadataTypeResult = await MetadataTypeRepositoryReader.GetMetadataTypeByName(name, cancellationToken);

        return metadataTypeResult.Match(
            item => Ok(item.ToMetadataTypeResponse()),
            Problem);
    }

}