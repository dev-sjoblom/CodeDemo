namespace CommunicationService.Classifications.Commands;

public static class ClassificationCommandErrors
{
    public static Error NameAlreadyExists => Error.Conflict(
        "Classification.NameAlreadyExists",
        $"Classification name already taken.");
}