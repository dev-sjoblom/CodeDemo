namespace CommunicationService.Fundamental.Errors;

public static class ErrorHelper
{

    public static Error NameAlreadyTakenError(string entityName) => Error.Conflict(
        code: ErrorHelper.NameAlreadyTakenName(entityName),
        description: ErrorHelper.NameAlreadyExistsDescription(entityName));
    public static string NameAlreadyTakenName(string entityName) =>
        $"{entityName}.{ErrorConstants.NameAlreadyExists}";
    public static string NameAlreadyExistsDescription(string entityName) =>
        $"{entityName} name already taken.";

    public static Error NotFoundError(string entityName) => Error.NotFound(
        code: ErrorHelper.NameNotFoundName(entityName),
        description: ErrorHelper.NameNotFoundDescription(entityName));
    public static string NameNotFoundName(string entityName) =>
        $"{entityName}.{ErrorConstants.ItemNotFound}";
    public static string NameNotFoundDescription(string entityName) =>
        $"{entityName} was not found.";

}