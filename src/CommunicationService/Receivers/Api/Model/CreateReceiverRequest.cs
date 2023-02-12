namespace CommunicationService.Receivers.Api.Model;

public record CreateReceiverRequest(string UniqueName, string Email, string[] Classifications, KeyValuePair<string, string>[] Metadata);