using CommunicationService.Fundamental.Controllers;
using CommunicationService.MetadataTypes.DataStore;

namespace CommunicationService.MetadataTypes.Fundamental;

public class MetadataTypeBase : ApiController
{
    public const string Route = "MetadataType";
    public const string GroupNaming = "MetadataType's";

    private const string GetByIdController = "GetMetadataTypeById";
    private const string GetByIdControllerAction = "GetMetadataTypeById";

    public MetadataTypeBase(ILogger logger) : base(logger)
    {
    }

    protected CreatedAtActionResult CreatedAtMetadataType(MetadataType metadataType)
    {
        return CreatedAtAction(
            controllerName: GetByIdController,
            actionName: GetByIdControllerAction,
            routeValues: new { id = metadataType.Id },
            value: metadataType.ToMetadataTypeResponse());
    }
}