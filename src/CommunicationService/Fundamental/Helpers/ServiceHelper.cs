using System.Reflection;
using CommunicationService.Fundamental.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
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

        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddValidatorsFromAssembly(typeof(Program).Assembly, ServiceLifetime.Singleton);

        services.ConfigureServices(connectionString);

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