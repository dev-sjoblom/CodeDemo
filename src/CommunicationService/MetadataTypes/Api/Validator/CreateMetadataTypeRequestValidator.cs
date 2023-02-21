using CommunicationService.MetadataTypes.Api.Model;
using CommunicationService.MetadataTypes.Data;
using FluentValidation;

namespace CommunicationService.MetadataTypes.Api.Validator;

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