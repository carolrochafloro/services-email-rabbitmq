using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FormContato.Context;
using FormContato.DTOs;
using FormContato.Repositories;
using AutoMapper;
using FormContato.Models;
using Microsoft.AspNetCore.Authorization;
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

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            var tokenFromCookie = Request.Cookies["jwt"];
            Console.WriteLine("Token recebido: " + tokenFromCookie);
            string userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            Console.WriteLine(userEmail);
            var contacts = await _unitOfWork.ContactRepository.GetAllAsync();
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

        // POST: Dashboard/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Email,Mensagem")] ContactDTO contactDTO)
        {
            if (ModelState.IsValid)
            {
                var newContact = _mapper.Map<ContactModel>(contactDTO);
                _unitOfWork.ContactRepository.Create(newContact);
                await _unitOfWork.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactDTO);
        }

        // GET: Dashboard/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactDTO = _unitOfWork.ContactRepository.Get(c => c.Id == id); // tornar async
            if (contactDTO == null)
            {
                return NotFound();
            }
            return View(contactDTO);
        }

        // POST: Dashboard/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nome,Email,Mensagem")] ContactDTO contactDTO)
        {
            if (id != contactDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var contact = _mapper.Map<ContactModel>(contactDTO);
                    _unitOfWork.ContactRepository.UpdateAsync(contact);
                    await _unitOfWork.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactDTOExists(contactDTO.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contactDTO);
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
