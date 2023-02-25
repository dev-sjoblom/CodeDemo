using CommunicationService.Receivers.DataAccess;

namespace CommunicationService.Receivers.Fundamental;

public class ReceiverBase : ApiController
{
    public const string Route = "Receiver";
    public const string GroupNaming = "Receiver's";

    private const string GetByIdController = "GetReceiverById";
    private const string GetByIdControllerAction = "GetReceiverById";
    
    public ReceiverBase(ILogger logger) : base(logger)
    {
        
    }
    protected CreatedAtActionResult CreatedAtReceiver(Receiver receiver)
    {
        return CreatedAtAction(
            controllerName: GetByIdController,
            actionName: GetByIdControllerAction,
            routeValues: new { id = receiver.Id },
            value: receiver.ToReceiverResponse());
    }
}