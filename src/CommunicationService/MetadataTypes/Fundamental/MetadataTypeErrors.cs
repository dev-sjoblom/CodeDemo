using CommunicationService.Fundamental.Errors;
using CommunicationService.MetadataTypes.DataAccess;

namespace CommunicationService.MetadataTypes.Fundamental;

public static class MetadataTypeErrors
{
    public static Error NotFound => 
        ErrorHelper.NotFoundError(MetadataTypeConstants.MetadataType);
    
    public static Error NameAlreadyExists => 
        ErrorHelper.NameAlreadyTakenError(MetadataTypeConstants.MetadataType);
}