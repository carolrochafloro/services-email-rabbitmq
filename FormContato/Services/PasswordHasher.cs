using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FormContato.Services;

public class PasswordHasher 
{
    // gerar um salt aleatório e passar como chave para HMACSHA25. usar computehash para gerar o hash da senha
    // após ter passado o salt como chave. converter arrays de bytes hash e salt para string e retornar os dois.
    public string[] HashPassword(string password)
    {
       
        byte[] salt = new byte[128 / 8];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var hmac = new HMACSHA256(salt);

        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] hash = hmac.ComputeHash(passwordBytes);

        string hashedPassword = BitConverter.ToString(hash);

        return [hashedPassword, BitConverter.ToString(salt)];
        
    }
   
}
