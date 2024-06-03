namespace Email;
using Email.Context;
using Email.Services;
using Microsoft.Extensions.DependencyInjection;

public class Worker : BackgroundService
{
    //private readonly ILogger<Worker> _logger;
    //private readonly IServiceScopeFactory _scopeFactory;
    //private readonly ILogger<Consumer> _loggerConsumer;

    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<Consumer> _loggerConsumer;
    private readonly ILogger<UpdateDB> _loggerDb;
    private readonly ILogger<SendEmail> _loggerSendEmail;

    public Worker(IServiceScopeFactory serviceScopeFactory, ILogger<Worker> logger, ILogger<Consumer> loggerConsumer, ILogger<UpdateDB> loggerDb, ILogger<SendEmail> loggerSendEmail)
    {
        _logger = logger;
        _scopeFactory = serviceScopeFactory;
        _loggerConsumer = loggerConsumer;
        _loggerDb = loggerDb;
        _loggerSendEmail = loggerSendEmail;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<EmailContext>();
            var consumer = new Consumer(context, _loggerConsumer, _loggerDb, _loggerSendEmail);

            await Task.Run(() => consumer.Consume());

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Run(() => consumer.HandleMessages());

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Consuming messages.");
                }

                await Task.Delay(1000, stoppingToken);
            }
        }

    }
}