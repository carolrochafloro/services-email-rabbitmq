using FormContato.Context;
using FormContato.Models;

namespace FormContato.Repositories;

public class UserRepository : Repository<UserModel>, IUserRepository
{
    public UserRepository(FCDbContext context) : base(context)
    {

    }


}
