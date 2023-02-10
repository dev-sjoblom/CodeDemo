using CommunicationService.Receivers.ContractModels;
using CommunicationService.Receivers.DataModels;

namespace CommunicationService.Receivers;

[ApiController]
[Route("[controller]")]
public class ReceiverController : ApiController
{
    private IReceiverRepository ReceiverRepository { get; }

    public ReceiverController(IReceiverRepository classificationRepository)
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
    
    [HttpGet]
    public async Task<IActionResult> ListReceivers(CancellationToken cancellationToken)
    {
        var classificationsResult  = await ReceiverRepository.ListReceivers(cancellationToken);

        return classificationsResult.Match(
            item => Ok(item.Select(x => x.ToReceiverResponse())),
            onError: Problem);
    }
    
    [HttpGet("ById/{id:guid}")]
    public async Task<IActionResult> GetReceiverById(Guid id, CancellationToken cancellationToken)
    {
        var classificationResult = await ReceiverRepository.GetReceiverById(id, cancellationToken);

        return classificationResult.Match(
            item => Ok(item.ToReceiverResponse()),
            Problem);
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
        var upsertedResult = await ReceiverRepository.UpsertReceiver(
            receiver, 
            request.Classifications, 
            request.Metadata,
            cancellationToken);

        var refreshReceiver = upsertedResult is { IsError: false, Value.RegisteredAsNewItem: true };
        if (refreshReceiver)
        {
            var getResult = await ReceiverRepository.GetReceiverById(receiver.Id, cancellationToken);
            if (getResult.IsError)
                return Problem(getResult.Errors);
            
            receiver = getResult.Value;
        }

        return upsertedResult.Match(
            item => item.RegisteredAsNewItem ? CreatedAtReceiver(receiver) : NoContent(),
            Problem);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteReceiver(Guid id, CancellationToken cancellationToken)
    {
        var deleteResult = await ReceiverRepository.DeleteReceiver(id, cancellationToken);

        return deleteResult.Match(
            _ => NoContent(),
            Problem);
    }

    private CreatedAtActionResult CreatedAtReceiver(Receiver receiver) => CreatedAtAction(
        actionName: nameof(GetReceiverById),
        routeValues: new { id = receiver.Id },
        value: receiver.ToReceiverResponse());
}