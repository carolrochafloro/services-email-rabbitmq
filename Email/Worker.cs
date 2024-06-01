namespace Email;
using dotenv.net;
using Email.Services;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        DotEnv.Load();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new Consumer();

        await Task.Run(() => consumer.Consume(Environment.GetEnvironmentVariable("QUEUE_NAME")));
        await Task.Run(() => consumer.ProcessMessages());

        while (!stoppingToken.IsCancellationRequested)
        {

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Consuming messages.");
            }
          
            await Task.Delay(1000, stoppingToken);
        }
    }

    // criar task para monitorar DB por mensagens não enviadas
    // e tentar novamente, atualizar o db
}
