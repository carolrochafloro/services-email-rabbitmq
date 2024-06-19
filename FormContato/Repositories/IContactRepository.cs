using FormContato.Models;
using System.Linq.Expressions;

namespace FormContato.Repositories;

public interface IContactRepository : IRepository<ContactModel>
{
    public Task<IEnumerable<ContactModel>> GetById(Expression<Func<ContactModel, bool>> predicate);
}
