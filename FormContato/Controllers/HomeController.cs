using FormContato.Context;
using FormContato.DTOs;
using FormContato.Models;
using FormContato.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        string queueName = "form_contact";

        try
        {
            _context.Add(model);
            await _context.SaveChangesAsync();
            var producer = new Producer();
            producer.Produce(model);
            return View("Success");
        }
        catch (Exception ex)
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
