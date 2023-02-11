using CommunicationService.Classifications.DataModels;

namespace CommunicationService.Receivers.DataModels;

public class ReceiverClassification
{
    public Guid ReceiverId { get; set; }
    public Guid ClassificationId { get; set; }
    public Receiver Receiver { get; set; } = null!;
    public Classification Classification { get; set; } = null!;
}