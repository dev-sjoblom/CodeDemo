namespace CommunicationService.Receivers.ContractModels;

public record CreateReceiverRequest(string UniqueName, string Email, string[] Classifications, KeyValuePair<string, string>[] Metadata);