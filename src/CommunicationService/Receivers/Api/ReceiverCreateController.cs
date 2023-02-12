using CommunicationService.Receivers.Api.Model;
using CommunicationService.Receivers.Data;
using CommunicationService.Receivers.Fundamental;

namespace CommunicationService.Receivers.Api;

[ApiController]
[Route("[controller]")]

public class ReceiverCreateController : ReceiverBaseController
{
    private IReceiverRepository ReceiverRepository { get; }

    public ReceiverCreateController(IReceiverRepository classificationRepository)
    {
        ReceiverRepository = classificationRepository;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateReceiver(CreateReceiverRequest request, CancellationToken cancellationToken)
    {
        var receiverResult = request.ToReceiver();
        if (receiverResult.IsError)
            return Problem(receiverResult.Errors);
        var receiver = receiverResult.Value;
        
        var createReceiverResult = await ReceiverRepository.CreateReceiver(
            receiver, 
            request.Classifications,
            request.Metadata, 
            cancellationToken);
        
        return createReceiverResult.Match(
            _ => CreatedAtReceiver(receiver),
            Problem);
    }
}