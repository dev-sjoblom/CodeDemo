using CommunicationService.Receivers.Data;

namespace CommunicationService.Receivers.Commands;

public static class ReceiverCommandErrors
{
    public static Error InvalidName => Error.Validation(
        "Receiver.InvalidName",
        $"Receiver unique name must be at least {Receiver.MinNameLength}" +
        $" characters long and at most {Receiver.MaxNameLength} characters long.");

    public static Error NameAlreadyExists => Error.Conflict(
        "Receiver.NameAlreadyExists",
        $"Receiver name already taken.");

    public static Error MetadataTypeNotAllowed => Error.Custom(StatusCodes.Status424FailedDependency,
        "Receiver.MetadataTypeNotAllowed",
        $"MetadataType not allowed on the receiver.");
}