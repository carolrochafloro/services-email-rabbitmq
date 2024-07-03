namespace FormContato.Services;

public interface IPasswordHasher
{
    public string Password { get; set; }
    public string Salt { get; set; }
    bool IsValidPassword(string password, string salt, string hash);
    void HashPassword(string password);
}
