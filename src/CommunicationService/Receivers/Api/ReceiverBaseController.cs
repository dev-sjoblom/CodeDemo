using CommunicationService.Receivers.Data;
using CommunicationService.Receivers.Fundamental;

namespace CommunicationService.Receivers.Api;

public class ReceiverBaseController : ApiController
{
    private const string _receiverGetByIdController = "ReceiverGetById";
    private const string _receiverGetByIdAction = "GetReceiverById";
    
    protected CreatedAtActionResult CreatedAtReceiver(Receiver receiver)
    {
        return CreatedAtAction(
            controllerName: _receiverGetByIdController,
            actionName: _receiverGetByIdAction,
            routeValues: new { id = receiver.Id },
            value: receiver.ToReceiverResponse());
    }
}