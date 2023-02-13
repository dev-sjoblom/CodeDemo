using CommunicationService.Classifications.Data;
using CommunicationService.MetadataTypes.Data;
using CommunicationService.Receivers.Core;
using CommunicationService.Receivers.Data;

namespace CommunicationService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        {
            builder.Services.AddDbContext<CommunicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("ServiceSqlConnectionString")));

            builder.Services.AddTransient<IClassificationRepositoryWriter, ClassificationRepositoryWriter>();
            builder.Services.AddTransient<IClassificationRepositoryReader, ClassificationRepositoryReader>();
            builder.Services.AddTransient<IMetadataTypeRepositoryWriter, MetadataTypeRepositoryWriter>();
            builder.Services.AddTransient<IMetadataTypeRepositoryReader, MetadataTypeRepositoryReader>();
            builder.Services.AddTransient<IReceiverRepositoryWriter, ReceiverRepositoryWriter>();
            builder.Services.AddTransient<IReceiverRepositoryReader, ReceiverRepositoryReader>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }

        var app = builder.Build();
        {
            if (app.Environment.IsDevelopment())
            {
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

            app.Run();
        }
    }
}