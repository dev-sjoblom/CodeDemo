namespace CommunicationService.Receivers.Commands;

public static class ReceiverCommandErrors
{
    public static Error NameAlreadyExists => Error.Conflict(
        "Receiver.NameAlreadyExists",
        $"Receiver name already taken.");

    public static Error MetadataTypeNotAllowed => Error.Custom(StatusCodes.Status424FailedDependency,
        "Receiver.MetadataTypeNotAllowed",
        $"MetadataType not allowed on the receiver.");
}