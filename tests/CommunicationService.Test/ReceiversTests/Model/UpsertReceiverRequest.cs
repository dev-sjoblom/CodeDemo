namespace CommunicationService.Test.ReceiversTests.Model;

public record UpsertReceiverRequest(
    string UniqueName,
    string Email,
    string[] Classifications, 
    KeyValuePair<string, string>[] Metadata);