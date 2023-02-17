using CommunicationService.Classifications.Data;

namespace CommunicationService.Classifications.Commands;

public static class ClassificationCommandErrors
{
    public static Error InvalidNameLength => Error.Validation(
        "Classification.InvalidNameLength",
        $"Classification name must be at least {Classification.MinNameLength}" +
        $" characters long and at most {Classification.MaxNameLength} characters long.");

    public static Error InvalidName => Error.Validation(
        "Classification.InvalidName",
        $"Classification name can only contain letters between a - z");

    public static Error NameAlreadyExists => Error.Conflict(
        "Classification.NameAlreadyExists",
        $"Classification name already taken.");
}