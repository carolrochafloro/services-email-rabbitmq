using AutoMapper;
using FormContato.DTOs;
using FormContato.Models;
using FormContato.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FormContato.Controllers
{

    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DashboardController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: Dashboard
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // pegar id do user do cookie, mostrar mensagens com userId == id
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userId is null)
            {
                TempData["Error"] = "Unable to obtain user ID. Please ensure you're logged in.";
                return RedirectToAction("Error", "Home");
            }
            var userGuid = Guid.Parse(userId.Value);
            
            var contacts =  _unitOfWork.ContactRepository.Get(c => c.UserId == userGuid);

            if (contacts is  null)
            {
                ViewBag.Message = "No contacts found.";
            }

            var contactDTOs = _mapper.Map<IEnumerable<ContactDTO>>(contacts);
            return View(contactDTOs);
        }

        // GET: Dashboard/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactDTO = _unitOfWork.ContactRepository.Get(m => m.Id == id);
            if (contactDTO == null)
            {
                return NotFound();
            }

            return View(contactDTO);
        }

        // GET: Dashboard/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: Dashboard/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactDTO = _unitOfWork.ContactRepository.Get(m => m.Id == id); // tornar async
            if (contactDTO == null)
            {
                return NotFound();
            }

            return View(contactDTO);
        }

        // POST: Dashboard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var contactDTO = _unitOfWork.ContactRepository.Get(u => u.Id == id); // tornar async
            if (contactDTO != null)
            {
                _unitOfWork.ContactRepository.DeleteAsync(contactDTO);
            }

            await _unitOfWork.CommitAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactDTOExists(Guid id)
        {
            var contact = _unitOfWork.ContactRepository.Get(c => c.Id == id);

            if (contact is null)
            {
                return false;
            }
            return true;
        }
    }
}
