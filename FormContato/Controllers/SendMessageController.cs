using AutoMapper;
using FormContato.DTOs;
using FormContato.Models;
using FormContato.Repositories;
using FormContato.Services;
using Microsoft.AspNetCore.Mvc;

namespace FormContato.Controllers;
public class SendMessageController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SendMessageController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        return View("SendMessage");
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
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Error", "Home");
        }

    }

}
