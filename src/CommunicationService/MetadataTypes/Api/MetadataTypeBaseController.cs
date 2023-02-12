using CommunicationService.MetadataTypes.Data;
using CommunicationService.MetadataTypes.Fundamental;

namespace CommunicationService.MetadataTypes.Api;

public class MetadataTypeBaseController : ApiController
{
    private const string _metadataTypeGetByIdController = "MetadataTypeGetById";
    private const string _metadataTypeGetByIdAction = "GetMetadataTypeById";
    
    protected CreatedAtActionResult CreatedAtMetadataType(MetadataType metadataType)
    {
        return CreatedAtAction(
            controllerName: _metadataTypeGetByIdController,
            actionName: _metadataTypeGetByIdAction,
            routeValues: new { id = metadataType.Id },
            value: metadataType.ToMetadataTypeResponse());
    }
}