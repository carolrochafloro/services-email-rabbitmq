
using FormContato.Context;
using FormContato.Models;

namespace FormContato.Logging;

public class DbLogger : ILogger
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DbLogger(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= LogLevel.Warning;
    }

    public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<FCDbContext>();
            var log = new LogModel

            {
                LogDate = DateTime.Now,
                LogLevel = logLevel.ToString(),
                LogType = typeof(TState).FullName,
                Source = exception?.Source,
                StackTrace = exception?.StackTrace,
            };

            dbContext.Logs.Add(log);
            await dbContext.SaveChangesAsync();
        }
    }
}
