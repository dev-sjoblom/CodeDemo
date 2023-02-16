using CommunicationService.MetadataTypes.Core;
using CommunicationService.MetadataTypes.Data;

namespace CommunicationService.MetadataTypes.Api;

[ApiController]
[Produces("application/json")]
[Route("[controller]")]
public class MetadataTypeGetByNameController : MetadataTypeBaseController
{
    private IMetadataTypeRepositoryReader MetadataTypeRepositoryReader { get; }

    public MetadataTypeGetByNameController(IMetadataTypeRepositoryReader metadataTypeRepositoryReader, ILogger<MetadataTypeGetByNameController> logger) : base(logger)
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