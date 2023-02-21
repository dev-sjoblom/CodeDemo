using CommunicationService.Classifications.Api.Model;
using CommunicationService.Classifications.Data;
using FluentValidation;

namespace CommunicationService.Classifications.Api.Validator;

public class CreateClassificationRequestValidator : AbstractValidator<CreateClassificationRequest>
{
    public CreateClassificationRequestValidator()
    {
        RuleFor(x => x.Name)
            .Length(
                ClassificationConstants.MinNameLength,
                ClassificationConstants.MaxNameLength)
            .Matches(ClassificationConstants.NameMatchRule)
            .WithMessage(ClassificationConstants.NamingDescription);

        RuleFor(x => x.MetadataTypes)
            .NotNull();
    }
}