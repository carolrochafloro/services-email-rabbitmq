using System.Linq.Expressions;

namespace FormContato.Repositories;

public interface IRepository<T>
{
    public Task<IEnumerable<T>> GetAllAsync();
    public T? Get(Expression<Func<T, bool>> predicate);
    public T Create(T entity);
    public T UpdateAsync(T entity);
    public T DeleteAsync(T entity);

}
