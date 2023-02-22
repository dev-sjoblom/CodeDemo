using CommunicationService.Classifications.DataStore;
using CommunicationService.Fundamental.Controllers;

namespace CommunicationService.Classifications.Fundamental;

public abstract class ClassificationBase : ApiController
{
    public const string Route = "Classification";
    public const string GroupNaming = "Classification's";
    private const string GetByIdController = "GetClassificationById";
    private const string GetByIdControllerAction = "GetClassificationById";

    protected ClassificationBase(ILogger logger) : base(logger)
    {
    }

    protected CreatedAtActionResult CreatedAtClassification(Classification classification)
    {
        return CreatedAtAction(
            controllerName: GetByIdController,
            actionName: GetByIdControllerAction,
            routeValues: new { id = classification.Id },
            value: classification.ToClassificationResponse());
    }
}