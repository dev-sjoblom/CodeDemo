using CommunicationService.Classifications.Api.Model;
using CommunicationService.Classifications.Data;
using FluentValidation;

namespace CommunicationService.Classifications.Api.Validator;

public class UpsertClassificationRequestValidator : AbstractValidator<UpsertClassificationRequest>
{
    public UpsertClassificationRequestValidator()
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