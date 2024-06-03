using FormContato.Context;

namespace FormContato.Logging;

public class DbLoggerProvider : ILoggerProvider
{
    private readonly FCDbContext _dbContext;

    public DbLoggerProvider(FCDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new DbLogger(_dbContext);
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
