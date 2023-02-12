using CommunicationService.Receivers.Data;

namespace CommunicationService.Receivers.Api;

[ApiController]
[ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class ReceiverDeleteController : ReceiverBaseController
{
    private IReceiverRepository ReceiverRepository { get; }

    public ReceiverDeleteController(IReceiverRepository classificationRepository)
    {
        ReceiverRepository = classificationRepository;
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteReceiver(Guid id, CancellationToken cancellationToken)
    {
        var deleteResult = await ReceiverRepository.DeleteReceiver(id, cancellationToken);

        return deleteResult.Match(
            _ => NoContent(),
            Problem);
    }
}