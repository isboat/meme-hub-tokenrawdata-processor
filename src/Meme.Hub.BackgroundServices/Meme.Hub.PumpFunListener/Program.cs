using Meme.Domain.Models.Settings;
using Meme.Hub.TokenRawDataProcessor.Models;
using Meme.Hub.TokenRawDataProcessor.Services;

namespace Meme.Hub.TokenRawDataProcessor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.Configure<PumpPortalSettings>(
                builder.Configuration.GetSection("PumpPortalSettings"));

            builder.Services.Configure<MessagingBusSettings>(
                builder.Configuration.GetSection("TokenRawDataMessagingBus"));


            builder.Services.AddControllers();

            // Register Hosted Services
            builder.Services.AddHostedService<RawDataQueueListener>();
            builder.Services.AddSingleton<IRawDataProcessor, RawDataProcessor>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
