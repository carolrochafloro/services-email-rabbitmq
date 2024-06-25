using AutoMapper;
using FormContato.DTOs;
using FormContato.Models;
using FormContato.Repositories;
using FormContato.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FormContato.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail is null)
            {
                return Forbid();
            }

            var user = await _unitOfWork.UserRepository.Get(u => u.Email == userEmail);

            if (user is null)
            {
                return Forbid();
            }

            var mappedUser = _mapper.Map<ProfileDTO>(user);
            ViewBag.User = mappedUser;
            return View(mappedUser);
        }



        // GET: User/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = _unitOfWork.UserRepository.Get(u => u.Id == id);

            if (userModel == null)
            {
                return NotFound();
            }
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] ProfileDTO newUser)
        {
            string stringId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid id = Guid.Parse(stringId);

            var user = await _unitOfWork.UserRepository.Get(u => u.Id == id);
            var hasher = new PasswordHasher();

            if (user is null)
            {
                return View("Error");
            }

            user.Email = newUser.Email;
            user.Name = newUser.Name;
            user.LastName = newUser.LastName;

            if (!hasher.IsValidPassword(newUser.Password, user.Salt, user.Password))
            {
                hasher.HashPassword(newUser.Password);

                user.Password = hasher.Password;
                user.Salt = hasher.Salt;
            }
                    
            user.LastUpdatedBy = id.ToString();
            user.LastUpdatedAt = DateTime.UtcNow;

            try
            {
                _unitOfWork.UserRepository.UpdateAsync(user);
                await _unitOfWork.CommitAsync();
                TempData["SuccessMessage"] = "Your profile was updated.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex;
                return View("Error");
            }
            
        }

        public async Task<IActionResult> DeleteUser()
        {

            try
            {
                string stringId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Guid id = Guid.Parse(stringId);
                var userModel = await _unitOfWork.UserRepository.Get(u => u.Id == id);

                if (userModel is null || userModel.IsActive == false)
                {
                    ViewBag.ErrorMessage = "User not found";
                }

                userModel.IsActive = false;
                userModel.LastUpdatedAt = DateTime.UtcNow;
                userModel.LastUpdatedBy = userModel.Id.ToString();
                _unitOfWork.UserRepository.UpdateAsync(userModel);

                await _unitOfWork.CommitAsync();

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }

        }

    }
}
