namespace CommunicationService.Test.ReceiversTests.ContractModels;

public record UpsertReceiverRequest(
    string UniqueName,
    string Email,
    string[] Classifications, 
    KeyValuePair<string, string>[] Metadata);