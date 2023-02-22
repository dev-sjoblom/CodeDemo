namespace CommunicationService.Receivers.Features.Upsert;

public record UpsertReceiverRequest(
    string UniqueName,
    string Email,
    string[] Classifications, 
    KeyValuePair<string, string>[] Metadata);