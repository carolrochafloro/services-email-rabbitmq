using FormContato.Context;
using FormContato.DTOs;
using FormContato.Models;
using FormContato.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FormContato.Controllers;
public class HomeController : Controller
{
    //private readonly ILogger<HomeController> _logger;
    protected readonly FCDbContext _context;

    public HomeController(FCDbContext context)
    {
        //_logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> SaveMessage(ContactDTO contato)
    {
        if (contato is null)
        {
            return View("Error");
        }

        ContactViewModel model = new ContactViewModel();

        model.Nome = contato.Nome;
        model.Email = contato.Email;
        model.Mensagem = contato.Mensagem;

        try
        {
            _context.Add(model);
            await _context.SaveChangesAsync();
            contato.Id = model.Id;
            var producer = new Producer();
            producer.Produce(contato);
            return View("Success");
        }
        catch (Exception)
        {
            return View("Error");
        }

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
