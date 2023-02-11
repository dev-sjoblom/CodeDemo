using CommunicationService.Receivers.ContractModels;
using CommunicationService.Receivers.DataModels;

namespace CommunicationService.Receivers;

public static class ReceiverConverter
{
    public static ReceiverResponse ToReceiverResponse(this Receiver receiver)
    {
        return new ReceiverResponse(
            receiver.Id,
            receiver.UniqueName,
            receiver.Email,
            receiver.Classifications.Select(x => x.Name).ToArray(),
            receiver.Metadatas.Select(x => 
                new ReceiverMetadataResponse( x.MetadataType.Name, x.Data)).ToArray());
    }
    public static ErrorOr<Receiver> ToReceiver(this UpsertReceiverRequest request, Guid id)
    {
        return Receiver.Create(request.UniqueName, request.Email, id: id);
    }

    public static ErrorOr<Receiver> ToReceiver(this CreateReceiverRequest request)
    {
        return Receiver.Create(request.UniqueName, request.Email);
    }
}