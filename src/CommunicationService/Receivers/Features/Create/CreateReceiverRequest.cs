namespace CommunicationService.Receivers.Features.Create;

public record CreateReceiverRequest(
    string UniqueName,
    string Email,
    string[] Classifications,
    KeyValuePair<string, string>[] Metadata);