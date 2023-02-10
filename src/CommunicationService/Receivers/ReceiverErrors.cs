using CommunicationService.Receivers.DataModels;

namespace CommunicationService.Receivers;

public static class ReceiverErrors
{
    public static Error InvalidName => Error.Validation(
        code: "Receiver.InvalidName",
        description: $"Receiver unique name must be at least {Receiver.MinNameLength}" +
                     $" characters long and at most {Receiver.MaxNameLength} characters long.");

    public static Error NotFound => Error.NotFound(
        code: "Receiver.NotFound",
        description: "Receiver not found");
}