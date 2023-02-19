using CommunicationService.Classifications.Data;

namespace CommunicationService.Receivers.Data;

public partial class Receiver
{
    public Guid Id { get; set; }
    public string UniqueName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<Classification> Classifications { get; set; }
    public List<ReceiverMetadata> Metadatas { get; set; }
}