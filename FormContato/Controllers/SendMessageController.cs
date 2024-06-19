using AutoMapper;
using FormContato.DTOs;
using FormContato.Models;
using FormContato.Repositories;
using FormContato.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;

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

    [HttpGet]
    public IActionResult Index([FromRoute] string encryptedEmail)
    {
        ViewBag.EncryptedEmail = encryptedEmail;
        return View("SendMessage");
    }

    [HttpPost]
    public async Task<IActionResult> SaveMessage([FromForm] ContactDTO contact, [FromRoute] string encryptedEmail)
    {
        if (contact is null)
        {
            return View("Error");
        }

        var newContact = _mapper.Map<ContactModel>(contact);

        newContact.SentTo = encryptedEmail;
        var userId = _unitOfWork.RecipientRepository.Get(r => r.Url == encryptedEmail);

        if (userId is null)
        {
            ModelState.AddModelError(string.Empty, "Failed attempt to login.");
            return View("Error");
        }

        newContact.UserId = userId.UserId;

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
