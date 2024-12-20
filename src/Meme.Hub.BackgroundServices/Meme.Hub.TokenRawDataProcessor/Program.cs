using Meme.Domain.Models.Settings;
using Meme.Hub.TokenRawDataProcessor.Interfaces;
using Meme.Hub.TokenRawDataProcessor.Services;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Meme.Hub.TokenRawDataProcessor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.Configure<MessagingBusSettings>(
                builder.Configuration.GetSection("TokenRawDataMessagingBus"));


            builder.Services.AddControllers();

            // Register Hosted Services
            builder.Services.AddHostedService<RawDataQueueListener>();
            builder.Services.AddSingleton<IRawDataProcessor, RawDataProcessor>();
            builder.Services.AddSingleton<ICacheService, RedisCacheService>();
            builder.Services.AddHttpClient<IDataHttpClient, DataHttpClient>();

            string connectionString = builder.Configuration.GetConnectionString("Redis");
            var multiplexer = ConnectionMultiplexer.Connect(connectionString!);
            builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
