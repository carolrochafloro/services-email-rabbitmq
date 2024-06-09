using AutoMapper;
using FormContato.Context;
using FormContato.DTOs;
using FormContato.Models;
using FormContato.Repositories;
using FormContato.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FormContato.Controllers;

// ideia: tirar o form de home e só permitir o acesso a ele com url personalizada com infos do usuário cadastado
public class HomeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public HomeController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> SaveMessage(ContactDTO contact)
    {
        if (contact is null)
        {
            return View("Error");
        }

        var newContact = _mapper.Map<ContactModel>(contact);

        try
        {
            _unitOfWork.ContactRepository.Create(newContact);
            await _unitOfWork.CommitAsync();
            var producer = new Producer();
            producer.Produce(newContact);
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
