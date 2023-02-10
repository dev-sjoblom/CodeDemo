namespace CommunicationService.Receivers.ContractModels;

public record ReceiverResponse (Guid Id, string UniqueName, string Email, string[] Classifications, ReceiverMetadataResponse[] Metadatas);

public record ReceiverMetadataResponse(string Key, string Data);