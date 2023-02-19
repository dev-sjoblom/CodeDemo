namespace CommunicationService.Receivers.Queries;

public static class ReceiverQueryErrors
{
    public static Error NotFound => Error.NotFound(
        "Receiver.NotFound",
        "Receiver not found");
}