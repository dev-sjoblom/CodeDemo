namespace CommunicationService.Fundamental.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {RequestName} with request: {@RequestData}",
            typeof(TRequest).Name,
            request);

        var response = await next();

        var responseLogLevel = GetLogLevelFromResponse(response);

        if (responseLogLevel != LogLevel.None)
            _logger.Log(responseLogLevel,
                "Error was found in response {@ResponseError}", response);

        return response;
    }

    private static LogLevel GetLogLevelFromResponse(TResponse response)
    {
        if (response is not IErrorOr errorOr || errorOr.Errors == null || !errorOr.IsError)
            return LogLevel.None;

        if (errorOr.Errors.Any(error => error.Type is ErrorType.Failure or ErrorType.Unexpected))
            return LogLevel.Error;

        if (errorOr.Errors.Any(error => error.Type is ErrorType.Conflict))
            return LogLevel.Warning;

        return LogLevel.Information;

    }
}