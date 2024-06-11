using dotenv.net;
using FormContato.DTOs;
using FormContato.Models;
using FormContato.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace FormContato.Services;

public class AuthenticateUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly PasswordHasher _passwordHasher;

    public AuthenticateUserService(IUnitOfWork unitOfWork, PasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        DotEnv.Load();
    }

    // conferir se existe, se a senha está correta, se está ativo, retornar result. ok
    // se result, gerar cookie ok
    // um método p/ logout 

    public async Task<Result> Authenticate(LoginDTO login)
    {

        try
        {
            var user = _unitOfWork.UserRepository.Get(u => u.Email == login.Email);

            if (user is null)
            {
                return new Result
                {
                    Success = false,
                    Error = "Invalid login attempt."
                };

            }

            bool isValidPassword = _passwordHasher.IsValidPassword(login.Password, user.Salt, user.Password);

            if (!isValidPassword)
            {
                return new Result
                {
                    Success = false,
                    Error = "Login or password are invalid."
                };
            }

            if (user.IsActive == false)
            {
                return new Result
                {
                    Success = false,
                    Error = "Invalid login attempt."
                };
            }
            return new Result
            {
                Success = true,
                User = user
            };
        }
        catch (Exception ex)
        {
            return new Result
            {
                Success = false,
                Error = ex.Message
            };
        }

    }

    public async Task<Result> GenerateCookies(UserModel user, HttpContext httpContext)
    {
        try
        {

            string userId = user.Id.ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.NameIdentifier, userId)
                // incluir role
            };

            var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                IsPersistent = true,
                //RedirectUri = $"{Environment.GetEnvironmentVariable("BASE_URL")}/Dashboard/Index"
            };

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return new Result
            {
                Success = true,
            };

        }
        catch (Exception ex)
        {
            return new Result
            {
                Success = false,
                Error = ex.Message
            };
        }

    }

}
