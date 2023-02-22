namespace CommunicationService.Receivers.Features;

public record ReceiverResponse(
    Guid Id,
    string UniqueName,
    string Email,
    string[] Classifications,
    ReceiverMetadataResponse[] Metadatas);

public record ReceiverMetadataResponse(
    string Key,
    string Data);