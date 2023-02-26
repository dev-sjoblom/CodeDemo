using CommunicationService.Classifications.DataAccess;

namespace CommunicationService.Receivers.DataAccess;

public class ReceiverClassification
{
    public Guid ReceiverId { get; set; }
    public Guid ClassificationId { get; set; }
    public Receiver Receiver { get; set; } = null!;
    public Classification Classification { get; set; } = null!;
}