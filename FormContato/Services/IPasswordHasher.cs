namespace FormContato.Services;

public interface IPasswordHasher
{
    bool IsValidPassword(string password, string salt, string hash);
    void HashPassword(string password);
}
