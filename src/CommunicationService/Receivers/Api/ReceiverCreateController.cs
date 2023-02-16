using CommunicationService.Receivers.Api.Model;
using CommunicationService.Receivers.Core;

namespace CommunicationService.Receivers.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ReceiverResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]

public class ReceiverCreateController : ReceiverBaseController
{
    private IReceiverRepositoryWriter ReceiverRepositoryWriter { get; }

    public ReceiverCreateController(IReceiverRepositoryWriter classificationRepositoryWriter, ILogger<ReceiverCreateController> logger) : base(logger)
    {
        ReceiverRepositoryWriter = classificationRepositoryWriter;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateReceiver(CreateReceiverRequest request, CancellationToken cancellationToken)
    {
        var receiverResult = request.ToReceiver();
        if (receiverResult.IsError)
            return Problem(receiverResult.Errors);
        var receiver = receiverResult.Value;
        
        var createReceiverResult = await ReceiverRepositoryWriter.CreateReceiver(
            receiver, 
            request.Classifications,
            request.Metadata, 
            cancellationToken);
        
        return createReceiverResult.Match(
            _ => CreatedAtReceiver(receiver),
            Problem);
    }
}