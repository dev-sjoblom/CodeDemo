using CommunicationService.Classifications;
using CommunicationService.MetadataTypes;
using CommunicationService.Receivers;

namespace CommunicationService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        {
            builder.Services.AddDbContext<CommunicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("ServiceSqlConnectionString")));

            builder.Services.AddTransient<IClassificationRepository, ClassificationRepository>();
            builder.Services.AddTransient<IMetadataTypeRepository, MetadataTypeRepository>();
            builder.Services.AddTransient<IReceiverRepository, ReceiverRepository>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }

        var app = builder.Build();
        {
            app.UseExceptionHandler("/error");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}