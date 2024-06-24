using AutoMapper;
using FormContato.DTOs;
using FormContato.Models;
using FormContato.Repositories;
using FormContato.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FormContato.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DashboardController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userId is null)
            {
                TempData["Error"] = "Unable to obtain user ID. Please ensure you're logged in.";
                return RedirectToAction("Error", "Home");
            }
            var userGuid = Guid.Parse(userId.Value);

            IEnumerable<ContactModel> contacts = await _unitOfWork.ContactRepository.GetById(c => c.UserId == userGuid);

            if (contacts is null)
            {
                ViewBag.Message = "No contacts found.";
            }

            var contactDTOs = _mapper.Map<IEnumerable<ContactDTO>>(contacts);

            return View(contactDTOs);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            // mostrar mensagem completa

            if (id == null)
            {
                ModelState.AddModelError("", "Contact ID must be informed.");
                return View();
            }

            var contact = await _unitOfWork.ContactRepository.Get(c => c.Id == id);

            if (contact is null)
            {
                ModelState.AddModelError("", "The contact doesn't exist.");
                return View();
            }

            return View(contact);
        }

        // GET: Dashboard/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                ModelState.AddModelError("", "Contact ID must be informed.");
                return View();
            }

            var contact = await _unitOfWork.ContactRepository.Get(c => c.Id == id);

            if (contact is null)
            {
                ModelState.AddModelError("", "The contact doesn't exist.");
                return View();
            }

            return View(contact);
        }

        // POST: Dashboard/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var contact = await _unitOfWork.ContactRepository.Get(c => c.Id == id); // tornar async

            if (contact is null)
            {
                ModelState.AddModelError("", "The contact doesn't exist.");
                return View();
            }

            _unitOfWork.ContactRepository.DeleteAsync(contact);
            await _unitOfWork.CommitAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UrlEmail(string email)
        {
            var shortenUrl = new ShortenURL();
            string? baseUrl = Environment.GetEnvironmentVariable("BASE_URL");
            string url;
            string bitlink;

            var recipient = await _unitOfWork.RecipientRepository.Get(r => r.RecipientEmail == email);
            var user = User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(user.Value);

            if (recipient == null || recipient.UserId != userId)
            {
                var encrypter = new EncryptedRecipientEmail();
                var result = await encrypter.Encrypt(email, userId.ToString());
                url = $"https://{baseUrl}/SendMessage/Index/{result.EncryptedEmail}";
                bitlink = await shortenUrl.GetShortUrl(url);

                var recipientObject = new RecipientModel
                {
                    RecipientEmail = email,
                    Url = url,
                    UserId = userId,
                };

                if (bitlink != Environment.GetEnvironmentVariable("BITLY_FAIL_RESPONSE"))
                {
                    recipientObject.ShortUrl = bitlink;
                }

                _unitOfWork.RecipientRepository.Create(recipientObject);
                await _unitOfWork.CommitAsync();

                ViewBag.Url = bitlink != Environment.GetEnvironmentVariable("BITLY_FAIL_RESPONSE") ? bitlink : url;

                return Content(bitlink != Environment.GetEnvironmentVariable("BITLY_FAIL_RESPONSE") ? bitlink : url);
            }

            bitlink = recipient.ShortUrl;

            ViewBag.Url = bitlink != null ? bitlink : recipient.Url;

            return Content(bitlink != null ? bitlink : recipient.Url);

        }

    }
}
