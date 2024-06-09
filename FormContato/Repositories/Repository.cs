using FormContato.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FormContato.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly FCDbContext _dbContext;

    public Repository(FCDbContext dbcontext)
    {
        _dbContext = dbcontext;
    }

    public T CreateAsync(T entity)
    {
        _dbContext.Set<T>().Add(entity);
        return entity;

    }

    public T? Get(Expression<Func<T, bool>> predicate)
    {
        return _dbContext.Set<T>().FirstOrDefault(predicate);
    }

    public IEnumerable<T> GetAllAsync()
    {
        return _dbContext.Set<T>().AsNoTracking().ToList();
    }

    public T UpdateAsync(T entity)
    {
        _dbContext.Set<T>().Update(entity);
        return entity;
    }

    public T DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        return entity;
    }

}
