using FormContato.Context;

namespace FormContato.Logging;

public class DbLoggerProvider : ILoggerProvider
{
   
    private readonly IServiceScopeFactory _serviceScopeFactory;


    public DbLoggerProvider(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new DbLogger(_serviceScopeFactory);
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
