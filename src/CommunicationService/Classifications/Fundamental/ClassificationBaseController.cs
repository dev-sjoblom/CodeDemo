using CommunicationService.Classifications.DataModels;

namespace CommunicationService.Classifications.Fundamental;

public abstract class ClassificationBaseController : ApiController
{
    protected CreatedAtActionResult CreatedAtClassification(Classification classification)
    {
        var GetByIdAction = ApiRouteHelper.ClassificationGetByIdRoute;
        return CreatedAtAction(
            actionName: GetByIdAction,
            routeValues: new { id = classification.Id },
            value: classification.ToClassificationResponse());
    }
}