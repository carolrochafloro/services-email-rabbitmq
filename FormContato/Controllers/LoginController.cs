using FormContato.DTOs;
using FormContato.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace FormContato.Controllers;
public class LoginController : Controller

{
    private readonly AuthenticateUserService _authService;

    public LoginController(AuthenticateUserService authService)
    {
        _authService = authService;
    }

    public IActionResult Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Dashboard");
        }

        return View("Login");
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDTO login)
    {

        try
        {

            var properties = await _authService.PrepareForAuthentication(login);

            var claimsPrincipal = new ClaimsPrincipal(properties.Item1);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                          claimsPrincipal, properties.Item2);

            return RedirectToAction("Index", "Dashboard");
        }

        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Error", "Home");
        }
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
