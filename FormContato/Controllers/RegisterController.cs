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
    private readonly PasswordHasher _hasher;

    public RegisterController(IMapper mapper, IUnitOfWork unitOfWork, PasswordHasher hasher)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _hasher = hasher;
    }

    public ActionResult Index()
    {
        return View("Register");
    }

    [HttpPost]
    public async Task<ActionResult> Create(RegisterDTO user)
    {
        try
        {

            var checkUser = _unitOfWork.UserRepository.Get(u => u.Email == user.Email);

            if (checkUser !=  null)
            {
                return BadRequest("This user is already registered.");
            }
 
            _hasher.HashPassword(user.Password);

            var newUser = _mapper.Map<UserModel>(user);

            newUser.Password = _hasher.Password;
            newUser.Salt = _hasher.Salt;
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
