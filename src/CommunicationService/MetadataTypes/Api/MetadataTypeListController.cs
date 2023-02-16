using CommunicationService.MetadataTypes.Api.Model;
using CommunicationService.MetadataTypes.Core;
using CommunicationService.MetadataTypes.Data;

namespace CommunicationService.MetadataTypes.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(MetadataTypeResponse[]), StatusCodes.Status200OK)]
[Route("[controller]")]
public class MetadataTypeListController : MetadataTypeBaseController
{
    private IMetadataTypeRepositoryReader MetadataTypeRepositoryReader { get; }

    public MetadataTypeListController(IMetadataTypeRepositoryReader metadataTypeRepositoryReader, ILogger<MetadataTypeListController> logger) : base(logger)
    {
        MetadataTypeRepositoryReader = metadataTypeRepositoryReader;
    }

    [HttpGet]
    public async Task<IActionResult> ListMetadataTypes(CancellationToken cancellationToken)
    {
        var metadataTypesResult = await MetadataTypeRepositoryReader.ListMetadataTypes(cancellationToken);

        return metadataTypesResult.Match(
            onValue: item => Ok(item.Select(x =>
                x.ToMetadataTypeResponse())),
            onError: Problem);
    }
}