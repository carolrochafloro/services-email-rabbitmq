namespace Email;
using Email.Context;
using Email.Services;
using Microsoft.Extensions.DependencyInjection;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public Worker(IServiceScopeFactory serviceScopeFactory, ILogger<Worker> logger)
    {
        _logger = logger;
        _scopeFactory = serviceScopeFactory;

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<EmailContext>();
            var consumer = new Consumer(context);

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
