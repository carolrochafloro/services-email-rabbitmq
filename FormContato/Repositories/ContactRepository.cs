using FormContato.Context;
using FormContato.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FormContato.Repositories;

public class ContactRepository : Repository<ContactModel>, IContactRepository
{
    public ContactRepository(FCDbContext context) : base(context)
    {

    }

    public async Task<IEnumerable<ContactModel>> GetById(Expression<Func<ContactModel, bool>> predicate)
    {

        return await _dbContext.Set<ContactModel>().Where(predicate).ToListAsync();

    }
}
