namespace FormContato.Repositories;

public interface IUnitOfWork
{
    IContactRepository ContactRepository { get; }
    IUserRepository UserRepository { get; }
    Task CommitAsync();
}
