﻿using dotenv.net;
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
    private readonly IPasswordHasher _passwordHasher;
    private readonly AuthenticationService _authService;

    public AuthenticateUserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, AuthenticationService authService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        DotEnv.Load();
        _authService = authService;
    }
    public async Task<Result> Authenticate(LoginDTO login)
    {
        // incluir verificação do model
        UserModel user = await FetchUser(login.Email);

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

    public async Task<(ClaimsIdentity, AuthenticationProperties)> PrepareForAuthentication(LoginDTO login)
    {
        UserModel user = await FetchUser(login.Email);

        if (user is not null)
        {
            var claimsIdentity = _authService.CreateClaimsIdentity(user);
            var authProperties = _authService.CreateAuthProperties();

            return (claimsIdentity, authProperties);
        }

        return (null, null);  
    }

    public async Task<UserModel> FetchUser(string email)
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


