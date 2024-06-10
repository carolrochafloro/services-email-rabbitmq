using dotenv.net;
using FormContato.Models;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FormContato.Services;

public class JwtHandler
{
    public JwtHandler()
    {
        DotEnv.Load();
    }

    private static ClaimsIdentity GenerateClaims(UserModel user)
    {
        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));

        return claimsIdentity;
    }
    public SecurityToken GenerateToken(UserModel user)
    {
        // gerar credentials com secret key e algoritmo.
        string? secretKey = Environment.GetEnvironmentVariable("JWT_PRIVATE_KEY");
        byte[] key = Encoding.ASCII.GetBytes(secretKey);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        // criar token descriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddHours(2),
            Subject = GenerateClaims(user),

        };

        var handler = new JwtSecurityTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);
        return token;
    }
}




