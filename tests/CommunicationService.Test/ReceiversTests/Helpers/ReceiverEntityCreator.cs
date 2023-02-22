using CommunicationService.Receivers.DataStore;

namespace CommunicationService.Test.ReceiversTests.Helpers;

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