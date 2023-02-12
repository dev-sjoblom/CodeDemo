using CommunicationService.Receivers.Data;

namespace CommunicationService.Test.ReceiversTests.Helpers;

public static class ReceiverEntityCreator
{
    public static Receiver CreateReceiver(string uniqueName, string email)
    {
        var receiverResult = Receiver.Create(uniqueName, email);
        receiverResult.IsError.Should().BeFalse(receiverResult.FirstError.Description);
        var receiver = receiverResult.Value;
        return receiver;
    }
}