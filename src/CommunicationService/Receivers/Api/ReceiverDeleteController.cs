using CommunicationService.Receivers.Data;

namespace CommunicationService.Receivers.Api;

[ApiController]
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