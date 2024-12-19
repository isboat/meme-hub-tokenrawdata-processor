using Azure.Messaging.ServiceBus;
using Meme.Domain.Models.Settings;
using Microsoft.Extensions.Options;


namespace Meme.Hub.TokenRawDataProcessor.Services
{
    public class RawDataQueueListener : BackgroundService
    {
        private MessagingBusSettings _messagingBusSettings;
        private readonly ILogger<RawDataQueueListener> _logger;
        private ServiceBusClient _client;
        private ServiceBusProcessor _processor;
        private readonly IRawDataProcessor _tokenRawDataProcessor;

        public RawDataQueueListener(
            IOptions<MessagingBusSettings> messageBusSettings,
            ILogger<RawDataQueueListener> logger,
            IRawDataProcessor tokenRawDataProcessor)
        {
            _logger = logger;
            _messagingBusSettings = messageBusSettings.Value;
            _tokenRawDataProcessor = tokenRawDataProcessor;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _client = new ServiceBusClient(_messagingBusSettings.ConnectionString);
            _processor = _client.CreateProcessor(_messagingBusSettings.QueueName, new ServiceBusProcessorOptions());

            _processor.ProcessMessageAsync += ProcessMessagesAsync;
            _processor.ProcessErrorAsync += ProcessErrorAsync;

            await _processor.StartProcessingAsync();

            _logger.LogInformation("Azure Service Bus Queue Listener started.");
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // No need to implement this for Service Bus processing
            await Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Azure Service Bus Queue Listener stopping.");

            await _processor.StopProcessingAsync();
            await _processor.DisposeAsync();
            await _client.DisposeAsync();

            await base.StopAsync(cancellationToken);
        }

        private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            string messageBody = args.Message.Body.ToString();
            await _tokenRawDataProcessor.ProcessTokenAsync(messageBody);

            // Complete the message. Message is deleted from the queue.
            await args.CompleteMessageAsync(args.Message);
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "Message handler encountered an exception");
            return Task.CompletedTask;
        }
    }

}
