using FormContato.DTOs;
using FormContato.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace FormContato.Controllers;
public class LoginController : Controller

// criar service para gerar JWT e refresh token e gerenciar sessão, criar model p/ sessão.
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
            var result = await _authService.Authenticate(login);

            if (result.Success == false || result.User == null)
            {
                ModelState.AddModelError(string.Empty, "Failed attempt to login.");
                return View("Login", login);
            }

            var cookieResult = await _authService.GenerateCookies(result.User, HttpContext);

            if (cookieResult.Success == false)
            {
                ModelState.AddModelError(string.Empty, "Failed attempt to login.");
                return View("Login", login);
            }

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
