using FormContato.Context;
using FormContato.Models;

namespace FormContato.Repositories;

public class RecipientRepository : Repository<RecipientModel>, IRecipientRepository
{
    public RecipientRepository(FCDbContext context) : base(context)
    {

    }

}
