using CommunicationService.Fundamental.DataAccess;
using CommunicationService.Fundamental.Logging;
using Serilog;

namespace CommunicationService.Fundamental.Helpers;

public static class ServiceSetupHelper
{
    public static WebApplication CreateAndConfigureWebApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        var configuration = builder.Configuration;

        builder.Services.AddOptions<DbOptions>()
            .Bind(configuration.GetSection(DbOptions.Section))
            .ValidateOnStart();

        var options = configuration.GetSection(DbOptions.Section).Get<DbOptions>() ??
                      throw new ApplicationException("DbOptions missing");
        var connectionString = options.GetConnectionString();

        builder.Host.ConfigureLogging();

        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddValidatorsFromAssembly(typeof(Program).Assembly, ServiceLifetime.Singleton);

        services.ConfigureDataAccess(connectionString);

        services.AddControllers();

        services.AddMediatR(option =>
            option.RegisterServicesFromAssembly(typeof(Program).Assembly));

        services.AddOpenApi();

        var app = builder.Build();

        app.UseSerilogRequestLogging();
        
        if (app.Environment.IsDevelopment())
        {
            if (!string.IsNullOrEmpty(connectionString))
                DatabaseHelper.CreateDatabaseIsMissing(connectionString);

            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/error");
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}