using CommunicationService.Classifications.DataAccess;
using CommunicationService.Fundamental.Errors;

namespace CommunicationService.Classifications.Fundamental;

public static class ClassificationErrors
{
    public static Error NotFound => 
        ErrorHelper.NotFoundError(ClassificationConstants.Classification);
    
    public static Error NameAlreadyExists => 
        ErrorHelper.NameAlreadyTakenError(ClassificationConstants.Classification);
}