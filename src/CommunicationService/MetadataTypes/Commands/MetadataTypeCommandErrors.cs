using CommunicationService.MetadataTypes.Data;

namespace CommunicationService.MetadataTypes.Commands;

public static class MetadataTypeCommandErrors
{
    public static Error InvalidName => Error.Validation(
        "MetadataType.InvalidName",
        $"MetadataType name must be at least {MetadataType.MinNameLength}" +
        $" characters long and at most {MetadataType.MaxNameLength} characters long.");

    public static Error NameAlreadyExists => Error.Conflict(
        "MetadataType.NameAlreadyExists",
        $"MetadataType name already taken.");
}