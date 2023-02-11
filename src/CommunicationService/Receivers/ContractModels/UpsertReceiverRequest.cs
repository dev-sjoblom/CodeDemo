namespace CommunicationService.Receivers.ContractModels;

public record UpsertReceiverRequest(
    string UniqueName,
    string Email,
    string[] Classifications, 
    KeyValuePair<string, string>[] Metadata);