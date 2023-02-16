using Serilog;

namespace CommunicationService.Fundamental.Helpers;

public static class ServiceHelper
{
    public static WebApplication CreateWebApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        
        builder.Host.ConfigureLogging();

        var connectionString = builder.Configuration.GetConnectionString("ServiceSqlConnectionString") ??
                               throw new ApplicationException("Config: ServiceSqlConnectionString is missing");
        
        services.ConfigureServices(connectionString);

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        var app = builder.Build();

        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
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