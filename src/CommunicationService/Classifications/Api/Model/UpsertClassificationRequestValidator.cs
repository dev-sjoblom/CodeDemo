using CommunicationService.Classifications.Data;
using FluentValidation;

namespace CommunicationService.Classifications.Api.Model;

public class UpsertClassificationRequestValidator : AbstractValidator<UpsertClassificationRequest>
{
    public UpsertClassificationRequestValidator()
    {
        RuleFor(x => x.Name)
            .Length(
                ClassificationConstants.MinNameLength,
                ClassificationConstants.MaxNameLength);

        RuleFor(x => x.MetadataTypes)
            .NotNull();
    }
}