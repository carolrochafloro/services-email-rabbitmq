using FormContato.Context;
using FormContato.Models;

namespace FormContato.Repositories;

public class ContactRepository: Repository<ContactViewModel>, IContactRepository
{
    public ContactRepository(FCDbContext context) : base(context)
    {

    }
}
