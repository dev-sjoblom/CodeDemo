using CommunicationService.Receivers.DataAccess;

namespace CommunicationService.Test.ReceiversTests.Fundamental;

public static class ReceiverEntityCreator
{
    public static Receiver CreateReceiver(string uniqueName, string email)
    {
        return new Receiver()
        {
            Id = Guid.NewGuid(),
            UniqueName = uniqueName,
            Email = email
        };
    }
}