using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using Lares.Entities;
using Lares.Interfaces;
using Lares.ViewModels;
using System.Security.Claims;
using Lares.Infrastructure.Repositories;
using Lares.Infrastructure;

namespace Lares.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IRepository<Property> _repo;

        public PropertyController(DataContext context)
        {
            _repo = new CoreRepository<Property>(context);
        }

        // GET: /property/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Property>>> Index()
        {
            var result = await _repo.GetAll();
            List<PropertyViewModel> allProps = new List<PropertyViewModel>();

            foreach (Property property in result)
            {
                PropertyViewModel propertyVM = new PropertyViewModel
                {
                    Id              = property.Id, 
                    OwnerUserId     = property.OwnerUserId,
                    OwnerUser       = property.OwnerUser,
                    Name            = property.Name,
                    Description     = property.Description,
                    Address1        = property.Address1,
                    Address2        = property.Address2,
                    AcquiredDate    = property.AcquiredDate
                };

                allProps.Add(propertyVM);
            }

            return View(allProps);
        }

        // GET: /property/create/
        public IActionResult Create()
        {
            PropertyViewModel propertyVM = new PropertyViewModel();
            propertyVM.AcquiredDate = DateTime.Now;
            return View(propertyVM);
        }

        // POST: /property/create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OwnerUserId,Name,Description,Address1,Address2,AcquiredDate")] PropertyViewModel propertyVM)
        {
            if(ModelState.IsValid)
            {
                Property newProperty = new Property
                {
                    OwnerUserId = propertyVM.OwnerUserId,
                    Name = propertyVM.Name,
                    Description = propertyVM.Description,
                    Address1 = propertyVM.Address1,
                    Address2 = propertyVM.Address2,
                    AcquiredDate = propertyVM.AcquiredDate
                };

                await _repo.AddAsync(newProperty);
                return RedirectToAction(nameof(Index));
            }

            return View(propertyVM);
        }

        // GET: /property/edit/[id]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _repo.GetByIdAsync((int)id);

            if (property == null)
            {
                return NotFound();
            }

            // map to the ViewModel
            PropertyViewModel propertyVM = new PropertyViewModel
            {
                Id = property.Id,
                OwnerUserId = property.OwnerUserId,
                Name = property.Name,
                Description = property.Description,
                Address1 = property.Address1,
                Address2 = property.Address2,
                AcquiredDate = property.AcquiredDate
            };

            return View(propertyVM);
        }

        // POST: /property/edit/[id]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OwnerUserId,Name,Description,Address1,Address2,AcquiredDate")] PropertyViewModel propertyVM)
        {
            if (id != propertyVM.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var property = await _repo.GetByIdAsync(id);

                if (property == null) return NotFound();

                // Map the ViewModel back to it's Model
                property.OwnerUserId = propertyVM.OwnerUserId;
                property.Name = propertyVM.Name;
                property.Description = propertyVM.Description;
                property.Address1 = propertyVM.Address1;
                property.Address2 = propertyVM.Address2;
                property.AcquiredDate = propertyVM.AcquiredDate;

                // Update the Property in the DB
                await _repo.UpdateAsync(property);

                return RedirectToAction(nameof(Index));
            }

            return View(propertyVM);
        }

        // GET: /property/delete/[id]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var property = await _repo.GetByIdAsync((int)id);

            if (property == null) return NotFound();

            return View(property);
        }

        // POST: /property/delete/[id]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
