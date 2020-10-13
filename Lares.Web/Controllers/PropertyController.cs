using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Lares.Entities;
using Lares.Infrastructure;
using Lares.ViewModels;

namespace Lares.Controllers
{
    public class PropertyController : Controller
    {
        private readonly DataContext _context;

        public PropertyController(DataContext context)
        {
            _context = context;
        }

        // GET: Property
        public async Task<IActionResult> Index()
        {
            return View(await _context.Property.ToListAsync());
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
            return View(propertyViewModel);
        }

        // POST: Property/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OwnerUserId,Name,Description,Address1,Address2,AcquiredDate,Id")] PropertyViewModel propertyViewModel)
        {
            if (ModelState.IsValid)
            {
                Property newProperty = new Property
                {
                    Id = propertyViewModel.Id,
                    OwnerUserId = propertyViewModel.OwnerUserId,
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
