
using FormContato.Context;
using FormContato.Models;

namespace FormContato.Logging;

public class DbLogger : ILogger
{
    private readonly FCDbContext _dbContext;

    public DbLogger(FCDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {

        var log = new LogModel
        {
            LogDate = DateTime.Now,
            LogLevel = logLevel.ToString(),
            LogType = typeof(TState).FullName,
            Source = exception?.Source,
            StackTrace = exception?.StackTrace,
        };

        _dbContext.Logs.Add(log);
        _dbContext.SaveChanges();
    }
}
