using AutoMapper;
using FormContato.DTOs;
using FormContato.Models;
using FormContato.Repositories;
using FormContato.Services;
using Microsoft.AspNetCore.Mvc;

namespace FormContato.Controllers;
[Route("SendMessage/[action]/{encryptedEmail}")]
public class SendMessageController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly Producer _producer;

    public SendMessageController(IUnitOfWork unitOfWork, IMapper mapper, Producer producer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _producer = producer;
    }

    public IActionResult Index()
    {
        return View("SendMessage");
    }

    [HttpPost]
    public async Task<IActionResult> SaveMessage([FromBody] ContactDTO contact, [FromRoute] string encryptedEmail)
    {
        if (contact is null)
        {
            return View("Error");
        }

        var newContact = _mapper.Map<ContactModel>(contact);

        try
        {
            var recipient = _unitOfWork.RecipientRepository.Get(r => r.Url == encryptedEmail);

            if (recipient is null)
            {
                return View("Error");
            }

            _unitOfWork.ContactRepository.Create(newContact);
            await _unitOfWork.CommitAsync();
         
            _producer.Produce(newContact, recipient);
            return View("Success");
        }

        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Error", "Home");
        }

    }

}
