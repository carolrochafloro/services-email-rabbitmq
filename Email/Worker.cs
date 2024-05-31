namespace Email;
using dotenv.net;
using Email.Services;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string queueName = "form_contact";
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumer = new Consumer();
            consumer.Consume(queueName);

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(consumer.message);
            }
            await Task.Delay(10000, stoppingToken);
        }
    }

    // criar task para monitorar DB por mensagens não enviadas
    // e tentar novamente, atualizar o db
}
