namespace CommunicationService.MetadataTypes.DataAccess;

public static class MetadataTypeConstants
{
    public const string MetadataType = "MetadataType";
    
    public const string IxMetadataTypeName = "IX_MetadataType_Name";

    public const int MinNameLength = 3;
    public const int MaxNameLength = 50;
    public const string NameMatchRule = "^[a-zA-Z]*$";
    public const string NamingDescription = "The name must only contain letters between a to z.";
}