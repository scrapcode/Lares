using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using Lares.Entities;
using Lares.Infrastructure;
using Lares.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Lares.Controllers
{
    public class PropertyController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PropertyController(DataContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Property
        public async Task<IActionResult> Index()
        {
            string currentUserId = _userManager.GetUserId(this.User);

            List<PropertyViewModel> propertyViewModels = new List<PropertyViewModel>();

            foreach (var property in await _context.Property.Where(p => p.OwnerUserId == currentUserId).ToListAsync())
            {
                // Fetch username for each property owner
                var ownerUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == property.OwnerUserId);
                string ownerUserName = ownerUser.UserName;

                // Map objects to their ViewModel
                PropertyViewModel tmpProperty = new PropertyViewModel
                {
                    Id = property.Id,
                    OwnerUserId = property.OwnerUserId,
                    Name = property.Name,
                    Description = property.Description,
                    Address1 = property.Address1,
                    Address2 = property.Address2,
                    AcquiredDate = property.AcquiredDate
                };

                // Add object to the list of items
                propertyViewModels.Add(tmpProperty);
            }

            if (TempData["Message"] != null)
            {
                if(TempData["MessageType"] == null)
                {
                    ViewBag.MessageType = "alert";
                }
                else
                {
                    ViewBag.MessageType = TempData["MessageType"];
                }

                ViewBag.Message = TempData["Message"];
            }

            return View(propertyViewModels);
        }

        // GET: Property/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @property = await _context.Property
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        // GET: Property/Create
        public IActionResult Create()
        {
            PropertyViewModel propertyViewModel = new PropertyViewModel();

            // Populate the OwnerSelectList with a list of users.
            var userList = _userManager.Users.Select(u => new { u.Id, u.UserName }).ToList();
            propertyViewModel.OwnerSelectList = new SelectList(userList, "Id", "UserName");

            // Set the default AcquiredDate to DateTime.Now
            propertyViewModel.AcquiredDate = DateTime.Now;

            return View(propertyViewModel);
        }

        // POST: Property/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OwnerUserId,Name,Description,Address1,Address2,AcquiredDate,Id")] PropertyViewModel propertyViewModel)
        {
            if (ModelState.IsValid)
            {
                var ownerUser = _userManager.Users.FirstOrDefault(u => u.Id == propertyViewModel.OwnerUserId);

                if (ownerUser == null) return NotFound();

                Property newProperty = new Property
                {
                    Id = propertyViewModel.Id,
                    OwnerUserId = propertyViewModel.OwnerUserId,
                    OwnerUser = ownerUser,
                    Name = propertyViewModel.Name,
                    Description = propertyViewModel.Description,
                    Address1 = propertyViewModel.Address1,
                    Address2 = propertyViewModel.Address2,
                    AcquiredDate = propertyViewModel.AcquiredDate
                };

                _context.Add(newProperty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(propertyViewModel);
        }

        // GET: Property/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @property = await _context.Property.FindAsync(id);
            if (@property == null)
            {
                return NotFound();
            }

            PropertyViewModel propertyViewModel = new PropertyViewModel
            {
                Id = @property.Id,
                OwnerUserId = @property.OwnerUserId,
                Name = @property.Name,
                Description = @property.Description,
                Address1 = @property.Address1,
                Address2 = @property.Address2,
                AcquiredDate = @property.AcquiredDate
            };

            return View(propertyViewModel);
        }

        // POST: Property/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OwnerUserId,Name,Description,Address1,Address2,AcquiredDate,Id")] PropertyViewModel propertyViewModel)
        {
            if (id != propertyViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Property editedProperty = await _context.Property
                    .FirstOrDefaultAsync(p => p.Id == id);

                editedProperty.OwnerUserId = propertyViewModel.OwnerUserId;
                editedProperty.Name = propertyViewModel.Name;
                editedProperty.Description = propertyViewModel.Description;
                editedProperty.Address1 = propertyViewModel.Address1;
                editedProperty.Address2 = propertyViewModel.Address2;
                editedProperty.AcquiredDate = propertyViewModel.AcquiredDate;

                try
                {
                    _context.Update(editedProperty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(editedProperty.Id))
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
            return View(propertyViewModel);
        }

        // GET: Property/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @property = await _context.Property
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        // POST: Property/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @property = await _context.Property.FindAsync(id);
            _context.Property.Remove(@property);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyExists(int id)
        {
            return _context.Property.Any(e => e.Id == id);
        }

        // TODO: GET: Property/SetCurrent/[id]
        [Authorize]
        public async Task<IActionResult> SetCurrent(int id)
        {
            var @property = await _context.Property.FindAsync(id);
            var currentUser = await _userManager.GetUserAsync(this.User);
            
            if( currentUser.Properties.Any( prop => prop.Equals(@property) ) )
            {
                currentUser.SelectedPropertyId = property.Id;
                await _userManager.UpdateAsync(currentUser);
                TempData["MessageType"] = "alert";
                TempData["Message"] = $"Your current property has changed to {property.Name}.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Property doesn't belong to the owner
                TempData["MessageType"] = "error";
                TempData["Message"] = $"The model selected to be default does not belong to the current user.";
                ModelState.AddModelError("Error", "The model selected to be default does not belong to the current user.");
                return RedirectToAction(nameof(Index));
            }
        }
        
    }
}
