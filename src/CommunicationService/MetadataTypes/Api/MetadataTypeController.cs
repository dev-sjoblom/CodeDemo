using CommunicationService.MetadataTypes.Models;

namespace CommunicationService.MetadataTypes;

[ApiController]
[Route("[controller]")]
public partial class MetadataTypeController : ApiController
{
    private IMetadataTypeRepository MetadataTypeRepository { get; }

    public MetadataTypeController(IMetadataTypeRepository metadataTypeRepository)
    {
        MetadataTypeRepository = metadataTypeRepository;
    }

    private CreatedAtActionResult CreatedAtGetMetadataType(MetadataType metadataType) => CreatedAtAction(
        actionName: nameof(GetMetadataTypeById),
        routeValues: new { id = metadataType.Id },
        value: metadataType.ToMetadataTypeResponse());
}