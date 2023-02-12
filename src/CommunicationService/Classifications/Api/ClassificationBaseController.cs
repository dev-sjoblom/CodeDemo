using CommunicationService.Classifications.Data;
using CommunicationService.Classifications.Fundamental;

namespace CommunicationService.Classifications.Api;

public abstract class ClassificationBaseController : ApiController
{
    private const string _classificationGetByIdController = "ClassificationGetById";
    private const string _classificationGetByIdAction = "GetClassificationById";
    
    protected CreatedAtActionResult CreatedAtClassification(Classification classification)
    {
        return CreatedAtAction(
            controllerName: _classificationGetByIdController,
            actionName: _classificationGetByIdAction,
            routeValues: new { id = classification.Id },
            value: classification.ToClassificationResponse());
    }
}