using CommunicationService.Classifications.Data;

namespace CommunicationService.Receivers.Data;

public class Receiver
{
    public required Guid Id { get; init; }
    public required string UniqueName { get; set; }
    public required string Email { get; set; }
    public List<Classification> Classifications { get; set; } = new();
    public List<ReceiverMetadata> Metadatas { get; set; } = new();
}