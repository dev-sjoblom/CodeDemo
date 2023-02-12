using CommunicationService.Classifications.Data;

namespace CommunicationService.Receivers.Data;

public class ReceiverClassification
{
    public Guid ReceiverId { get; set; }
    public Guid ClassificationId { get; set; }
    public Receiver Receiver { get; set; } = null!;
    public Classification Classification { get; set; } = null!;
}