using CommunicationService.MetadataTypes.DataAccess;

namespace CommunicationService.MetadataTypes.Features.Create;

public class CreateMetadataTypeRequestValidator : AbstractValidator<CreateMetadataTypeRequest>
{
    public CreateMetadataTypeRequestValidator()
    {
        RuleFor(x => x.Name)
            .Length(
                MetadataTypeConstants.MinNameLength,
                MetadataTypeConstants.MaxNameLength)
            .Matches(MetadataTypeConstants.NameMatchRule)
            .WithMessage(MetadataTypeConstants.NamingDescription);

        RuleFor(x => x.Classifications)
            .NotNull();
    }
}