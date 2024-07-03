using AutoMapper;
using FormContato.DTOs;
using FormContato.Models;
using FormContato.Repositories;
using FormContato.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Protocol.Plugins;
using System.Security.Claims;

namespace FormContato.Controllers;
public class RegisterController : Controller
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _hasher;
    private readonly AuthenticateUserService _authService;

    public RegisterController(IMapper mapper, IUnitOfWork unitOfWork, IPasswordHasher hasher, AuthenticateUserService authService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _hasher = hasher;
        _authService = authService;
    }
    // removendo var model para ver se corrijo o bug
    public ActionResult Index(LoginDTO login = null)
    {
        var model = login != null ? new RegisterDTO { Email = login.Email, Password = login.Password } : new RegisterDTO();
        return View("Register");
    }

    [HttpPost]
    public async Task<ActionResult> Create(RegisterDTO user)
    {
        try
        {

            var checkUser = _unitOfWork.UserRepository.Get(u => u.Email == user.Email);

            if (checkUser != null)
            {
                return RedirectToAction("Index", "Login");
            }

            _hasher.HashPassword(user.Password);

            var newUser = _mapper.Map<UserModel>(user);

            newUser.Password = _hasher.Password;
            newUser.Salt = _hasher.Salt;
            newUser.Role = RoleEnum.User;

            _unitOfWork.UserRepository.Create(newUser);
            await _unitOfWork.CommitAsync();

            var newUserLogin = new LoginDTO
            {
                Email = user.Email,
                Password = user.Password,
            };
            // prepare for auth
            // httpcontext p/ login

            var properties = await _authService.PrepareForAuthentication(newUserLogin);

            var claimsPrincipal = new ClaimsPrincipal(properties.Item1);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                          claimsPrincipal, properties.Item2);

            return RedirectToAction("Index", "Dashboard"); // redirecionar para página inicial de usuário logado
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Error", "Home");
        }
    }

}
