using CommunicationService.Receivers.Api.Model;

namespace CommunicationService.Receivers.Data;

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
                new ReceiverMetadataResponse(x.MetadataType.Name, x.Data)).ToArray());
    }
}