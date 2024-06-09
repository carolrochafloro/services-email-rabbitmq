
using FormContato.Context;

namespace FormContato.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IContactRepository _contactRepository;
    private IUserRepository _userRepository;
    public FCDbContext _context;

    public UnitOfWork(FCDbContext context)
    {
        _context = context;
    }

    public IContactRepository ContactRepository
    {
        get
        {
            return _contactRepository = _contactRepository ?? new ContactRepository(_context);
        }
    }

    public IUserRepository UserRepository
    {
        get
        {
            return _userRepository = _userRepository ?? new UserRepository(_context);
        }
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
