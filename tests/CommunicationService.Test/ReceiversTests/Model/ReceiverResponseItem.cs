namespace CommunicationService.Test.ReceiversTests.Model;

public record ReceiverResponseItem (
    Guid Id, 
    string UniqueName, 
    string Email, 
    string[] Classifications, 
    ReceiverMetadataResponseItem[] Metadatas);

public record ReceiverMetadataResponseItem(
    string Key, 
    string Data);