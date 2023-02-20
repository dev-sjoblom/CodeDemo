using FluentValidation.Results;

namespace CommunicationService.Fundamental.Helpers;

public static class ValidatorHelper
{
    public static IDictionary<string, string[]> ToDictionary2(this ValidationResult validationResult)
    {
        return validationResult.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray()
            );
    }
}