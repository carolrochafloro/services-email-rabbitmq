namespace FormContato.Repositories;

public interface IUnitOfWork
{
    IContactRepository ContactRepository { get; }
    IUserRepository UserRepository { get; }
    IRecipientRepository RecipientRepository { get; }
    Task CommitAsync();
}
