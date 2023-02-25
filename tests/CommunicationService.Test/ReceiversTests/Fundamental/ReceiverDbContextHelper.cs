using CommunicationService.Fundamental.DataAccess;
using CommunicationService.Receivers.DataAccess;
using CommunicationService.Test.ClassificationTests.Fundamental;

namespace CommunicationService.Test.ReceiversTests.Fundamental;

public static class ReceiverDbContextHelper
{
    public static Receiver AddReceiverWithClassifications(this CommunicationDbContext dbContext,
        string uniqueName, string email, string[] classifications)
    {
        var receiver = ReceiverEntityCreator.CreateReceiver(uniqueName, email);
        dbContext.Receiver.Add(receiver);
        dbContext.AddClassificationToReceiver(receiver, classifications);
        
        return receiver;
    }

    public static Receiver AddReceiverWithMetadata(this CommunicationDbContext dbContext,
        string uniqueName, 
        string email, 
        string[] classifications,
        string metadataTypeName,
        string metadataValue)
    {
        var receiver = dbContext.AddReceiverWithClassifications(uniqueName, email, classifications);
        var metadataType = dbContext.AddMetadataType(metadataTypeName);

        var metadata = new ReceiverMetadata
        {
            ReceiverId = receiver.Id,
            MetadataTypeId = metadataType.Id,
            Data = metadataValue
        };
        receiver.Metadatas.Add(metadata);

        return receiver;
    }

    private static void AddClassificationToReceiver(this CommunicationDbContext dbContext,
        Receiver receiver, string[] classifications)
    {
        if (classifications == null)
            return;
        
        foreach (var c in classifications)
        {
            var classification = dbContext.AddClassification(c);
            receiver.Classifications.Add(classification);
        }
    }
}