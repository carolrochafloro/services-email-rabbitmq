using System.Security.Cryptography;
using System.Text;

namespace FormContato.Services;

public class PasswordHasher : IPasswordHasher
{
   
    public string Password { get; set; }
    public string Salt { get; set; }
    public void HashPassword(string password)
    {

        byte[] salt = new byte[128 / 8];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var hmac = new HMACSHA256(salt);

        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] hash = hmac.ComputeHash(passwordBytes);

        Salt = Convert.ToBase64String(salt);
        Password = Convert.ToBase64String(hash);

    }

    public bool IsValidPassword(string password, string salt, string storedHash)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        var hmac = new HMACSHA256(saltBytes);
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] computedHash = hmac.ComputeHash(passwordBytes);
        string computedHashString = Convert.ToBase64String(computedHash);

        return computedHashString.Equals(storedHash);
    }

}
