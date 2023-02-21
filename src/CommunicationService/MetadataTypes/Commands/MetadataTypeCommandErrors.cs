namespace CommunicationService.MetadataTypes.Commands;

public static class MetadataTypeCommandErrors
{
    public static Error NameAlreadyExists => Error.Conflict(
        "MetadataType.NameAlreadyExists",
        $"MetadataType name already taken.");
}