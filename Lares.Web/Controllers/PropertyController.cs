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

namespace Lares.Controllers
{
    public class PropertyController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public PropertyController(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Property
        public async Task<IActionResult> Index()
        {
            List<PropertyViewModel> propertyViewModels = new List<PropertyViewModel>();

            foreach (var property in await _context.Property.ToListAsync())
            {
                // Fetch username for each property owner
                var ownerUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == property.OwnerUserId);
                string ownerUserName = ownerUser.UserName;

                // Map objects to their ViewModel
                PropertyViewModel tmpProperty = new PropertyViewModel
                {
                    Id = property.Id,
                    OwnerUserId = property.OwnerUserId,
                    OwnerUserName = ownerUserName,
                    Name = property.Name,
                    Description = property.Description,
                    Address1 = property.Address1,
                    Address2 = property.Address2,
                    AcquiredDate = property.AcquiredDate
                };

                // Add object to the list of items
                propertyViewModels.Add(tmpProperty);
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
    }
}
