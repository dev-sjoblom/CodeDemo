using CommunicationService.Fundamental.Controllers;
using CommunicationService.MetadataTypes.Core;
using CommunicationService.MetadataTypes.Data;

namespace CommunicationService.MetadataTypes.Api;

public class MetadataTypeBaseController : ApiController
{
    private const string _metadataTypeGetByIdController = "MetadataTypeGetById";
    private const string _metadataTypeGetByIdAction = "GetMetadataTypeById";

    public MetadataTypeBaseController(ILogger logger) : base(logger)
    {
        
    }
    
    protected CreatedAtActionResult CreatedAtMetadataType(MetadataType metadataType)
    {
        return CreatedAtAction(
            controllerName: _metadataTypeGetByIdController,
            actionName: _metadataTypeGetByIdAction,
            routeValues: new { id = metadataType.Id },
            value: metadataType.ToMetadataTypeResponse());
    }
}