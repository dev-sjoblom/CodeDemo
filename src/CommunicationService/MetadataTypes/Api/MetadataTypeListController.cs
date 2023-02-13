using CommunicationService.MetadataTypes.Api.Model;
using CommunicationService.MetadataTypes.Data;
using CommunicationService.MetadataTypes.Fundamental;

namespace CommunicationService.MetadataTypes.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(MetadataTypeResponse[]), StatusCodes.Status200OK)]
[Route("[controller]")]
public class MetadataTypeListController : MetadataTypeBaseController
{
    private IMetadataTypeRepositoryReader MetadataTypeRepositoryReader { get; }

    public MetadataTypeListController(IMetadataTypeRepositoryReader metadataTypeRepositoryReader)
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