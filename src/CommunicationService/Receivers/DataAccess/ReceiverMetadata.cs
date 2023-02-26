using CommunicationService.MetadataTypes.DataAccess;

namespace CommunicationService.Receivers.DataAccess;

public class ReceiverMetadata
{
    public Guid ReceiverId { get; set; }
    public Guid MetadataTypeId { get; set; }
    public string Data { get; set; } = null!;
    public Receiver Receiver { get; set; } = null!;
    public MetadataType MetadataType  { get; set; } = null!;
}