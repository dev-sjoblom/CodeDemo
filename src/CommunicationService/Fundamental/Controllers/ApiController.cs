using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CommunicationService.Fundamental.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class ApiController : ControllerBase
{
    protected ILogger Logger { get; }

    public ApiController(ILogger logger)
    {
        Logger = logger;
    }

    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in errors) modelStateDictionary.AddModelError(error.Code, error.Description);

            return ValidationProblem(modelStateDictionary);
        }

        if (errors.Any(e => e.Type == ErrorType.Unexpected)) return Problem();

        var firstError = errors[0];

        var statusCode = firstError switch
        {
            { Type: ErrorType.NotFound } => StatusCodes.Status404NotFound,
            { Type: ErrorType.Validation } => StatusCodes.Status400BadRequest,
            { Type: ErrorType.Conflict } => StatusCodes.Status409Conflict,
            { NumericType: StatusCodes.Status424FailedDependency } => StatusCodes.Status424FailedDependency,
            _ => StatusCodes.Status500InternalServerError
        };
        
        Logger.LogInformation("Managed error occurred with status code {StatusCode} in request:{@Error}", statusCode, errors);

        return Problem(statusCode: statusCode, title: firstError.Description);
    }
}