namespace CommunicationService.Test.ReceiversTests.Response;

public record ReceiverResponseItem (
    Guid Id, 
    string UniqueName, 
    string Email, 
    string[] Classifications, 
    ReceiverMetadataResponseItem[] Metadatas);

public record ReceiverMetadataResponseItem(
    string Key, 
    string Data);