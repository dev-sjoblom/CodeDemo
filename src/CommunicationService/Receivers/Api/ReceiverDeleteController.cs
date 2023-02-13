using CommunicationService.Receivers.Core;
using CommunicationService.Receivers.Data;

namespace CommunicationService.Receivers.Api;

[ApiController]
[ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class ReceiverDeleteController : ReceiverBaseController
{
    private IReceiverRepositoryWriter ReceiverRepositoryWriter { get; }

    public ReceiverDeleteController(IReceiverRepositoryWriter classificationRepositoryWriter)
    {
        ReceiverRepositoryWriter = classificationRepositoryWriter;
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteReceiver(Guid id, CancellationToken cancellationToken)
    {
        var deleteResult = await ReceiverRepositoryWriter.DeleteReceiver(id, cancellationToken);

        return deleteResult.Match(
            _ => NoContent(),
            Problem);
    }
}