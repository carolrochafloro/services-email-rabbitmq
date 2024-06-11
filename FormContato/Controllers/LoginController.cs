using AutoMapper;
using FormContato.DTOs;
using FormContato.Repositories;
using FormContato.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FormContato.Controllers;
public class LoginController : Controller

    // criar service para gerar JWT e refresh token e gerenciar sessão, criar model p/ sessão.
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtHandler _jwtHandler;

    public LoginController(IUnitOfWork unitOfWork, JwtHandler jwtHandler)
    {
        _unitOfWork = unitOfWork;
        _jwtHandler = jwtHandler;
    }

    public IActionResult Index()
    {
        return View("Login");
    }

    [HttpPost]
    public IActionResult Login(LoginDTO login)
    {
        if (login is null)
        {
            return BadRequest("All data must be provided.");
        }
        try
        {
            var user = _unitOfWork.UserRepository.Get(u => u.Email == login.Email);

            if (user is null)
            {
                return RedirectToAction("Index", "Register", login);
            }

            var hasher = new PasswordHasher();

            bool isValidPassword = hasher.ComparePassword(login.Password, user.Salt, user.Password);

            if (!isValidPassword)
            {
                ModelState.AddModelError(string.Empty, "Incorrect user or password.");
                return View("Login", login);
            }

            var token = _jwtHandler.GenerateToken(user);
            string tokenString = _jwtHandler.WriteToken(token);

            if (string.IsNullOrEmpty( tokenString ))
            {
                TempData["Error"] = "The authentication failed.";
                return RedirectToAction("Error", "Home");
            }

            Console.WriteLine("Token gerado: " + token); // Imprime o token gerado

            Response.Cookies.Append("jwt", tokenString, new CookieOptions { HttpOnly = true, Secure = true });
            return RedirectToAction("Index", "Dashboard");

        } catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Error", "Home");
        }
    }
}
