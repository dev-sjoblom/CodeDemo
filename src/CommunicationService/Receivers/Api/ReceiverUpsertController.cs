using CommunicationService.Receivers.Api.Model;
using CommunicationService.Receivers.Core;

namespace CommunicationService.Receivers.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ReceiverResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
[Route("[controller]")]
public class ReceiverUpsertController : ReceiverBaseController
{
    private IReceiverRepositoryWriter ReceiverRepositoryWriter { get; }

    public ReceiverUpsertController(IReceiverRepositoryWriter classificationRepositoryWriter, ILogger<ReceiverUpsertController> logger) : base(logger)
    {
        ReceiverRepositoryWriter = classificationRepositoryWriter;
    }
    
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpsertReceiver(Guid id, UpsertReceiverRequest request, CancellationToken cancellationToken)
    {
        var receiverResult = request.ToReceiver(id);

        if (receiverResult.IsError)
        {
            return Problem(receiverResult.Errors);
        }

        var receiver = receiverResult.Value;
        var upsertedResult = await ReceiverRepositoryWriter.UpsertReceiver(
            receiver, 
            request.Classifications, 
            request.Metadata,
            cancellationToken);
        
        return upsertedResult.Match(
            item => item.RegisteredAsNewItem ? CreatedAtReceiver(receiver) : NoContent(),
            Problem);
    }

}