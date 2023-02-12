using CommunicationService.MetadataTypes.Data;
using static CommunicationService.Classifications.Fundamental.ClassificationErrors;

namespace CommunicationService.Classifications.Data;

public partial class Classification
{
    private Classification()
    {
        MetadataTypes = new List<MetadataType>();
    }

    private Classification(string name, Guid? id = null) : this()
    {
        Name = name;
        Id = id ?? Guid.NewGuid();
    }
    
    public static ErrorOr<Classification> Create(
        string name,
        Guid? id = null)
    {
        List<Error> errors = new();

        if (name.Length is < MinNameLength or > MaxNameLength)
        {
            errors.Add(InvalidNameLength);
        }

        if (!name.All(c => Char.IsLetter(c) && (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z')))
        {
            errors.Add(InvalidName);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Classification(name, id: id);
    }
}