using FormContato.DTOs;
using FormContato.Repositories;
using Microsoft.AspNetCore.Mvc;
using FormContato.Services;
using FormContato.Models;
using AutoMapper;

namespace FormContato.Controllers;
public class RegisterController : Controller
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterController(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    // GET: RegisterController
    public ActionResult Index()
    {
        return View("Register");
    }

    // POST: RegisterController/Create - Cadastrar
    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(RegisterDTO user)
    {
        try
        {
 
            PasswordHasher hasher = new PasswordHasher();
            hasher.HashPassword(user.Password);

            var newUser = _mapper.Map<UserModel>(user);

            newUser.Password = hasher.Password;
            newUser.Salt = hasher.Salt;
            newUser.Role = RoleEnum.User;

            _unitOfWork.UserRepository.Create(newUser);
            await _unitOfWork.CommitAsync();

            return RedirectToAction(nameof(Index)); // redirecionar para página inicial de usuário logado
        }
        catch
        {
            return View("Error");
        }
    }

}
