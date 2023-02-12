using CommunicationService.Classifications.DataModels;
using static System.Char;
using static CommunicationService.MetadataTypes.MetadataTypeErrors;

namespace CommunicationService.MetadataTypes.DataModels;

public partial class MetadataType
{
    
    private MetadataType()
    {
        Classifications = new List<Classification>();
    }
    
    private MetadataType(string name, Guid? id = null)
    {
        Name = name;
        Id = id ?? Guid.NewGuid();
        Classifications = new List<Classification>();
    }
    
    public static ErrorOr<MetadataType> Create(
        string name,
        Guid? id = null)
    {
        List<Error> errors = new();

        if (name.Length is < MinNameLength or > MaxNameLength)
        {
            errors.Add(InvalidName);
        }
        
        if (!name.All(c => IsLetter(c) && ToLower(c) is >= 'a' and <= 'z'))
        {
            errors.Add(InvalidName);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new MetadataType(name, id: id);
    }
}