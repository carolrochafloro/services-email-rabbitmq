using AutoMapper;
using FormContato.DTOs;
using FormContato.Repositories;
using FormContato.Services;
using Microsoft.AspNetCore.Mvc;

namespace FormContato.Controllers;
public class LoginController : Controller

    // criar service para gerar JWT e refresh token e gerenciar sessão, criar model p/ sessão.
{
    private readonly IUnitOfWork _unitOfWork;

    public LoginController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        return View("Login");
    }

    [HttpPost]
    public ActionResult Login(LoginDTO login)
    {
        if (login is null)
        {
            return BadRequest("All the data must be provided.");
        }
        try
        {
            var user = _unitOfWork.UserRepository.Get(u => u.Email == login.Email);

            if (user is null)
            {
                return BadRequest("User not found.");
            }

            var hasher = new PasswordHasher();

            bool isValidPassword = hasher.ComparePassword(login.Password, user.Salt, user.Password);

            if (!isValidPassword)
            {
                ModelState.AddModelError(string.Empty, "Incorrect user or password.");
                return View("Login", login);
            }

            return View("Login"); // retornar página de usuário logado

        } catch (Exception)
        {
            return View("Error");
        }
    }
}
