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
    private readonly AuthenticationService _authService;

    public AuthenticateUserService(IUnitOfWork unitOfWork, PasswordHasher passwordHasher, AuthenticationService authService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        DotEnv.Load();
        _authService = authService;
    }
    public async Task<Result> Authenticate(LoginDTO login)
    {
        var user = await FetchUser(login.Email);

        if (user is null)
        {
            return UserNotFoundError();
        }

        if (!IsValidPassword(login.Password, user))
        {
            return InvalidPasswordError();
        }

        if (!user.IsActive)
        {
            return UserNotActiveError();
        }

        return SuccessfulResult(user);
    }

    public async Task<Result> GenerateCookies(UserModel user, HttpContext httpContext)
    {
        try
        {
            var claimsIdentity = _authService.CreateClaimsIdentity(user);
            var authProperties = _authService.CreateAuthProperties();

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

    public async Task<UserModel> FetchUser (string email)
    {
        return await _unitOfWork.UserRepository.Get(u => u.Email == email);
    }

    private bool IsValidPassword(string password, UserModel user)
    {
        return _passwordHasher.IsValidPassword(password, user.Salt, user.Password);
    }

    private Result UserNotFoundError()
    {
        return new Result
        {
            Success = false,
            Error = "Invalid login attempt."
        };
    }

    private Result InvalidPasswordError()
    {
        return new Result
        {
            Success = false,
            Error = "Login or password are invalid."
        };
    }

    private Result UserNotActiveError()
    {
        return new Result
        {
            Success = false,
            Error = "Invalid login attempt."
        };
    }

    private Result SuccessfulResult(UserModel user)
    {
        return new Result
        {
            Success = true,
            User = user
        };
    }

    
}


