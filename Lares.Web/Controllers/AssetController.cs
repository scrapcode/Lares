using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lares.Entities;
using Lares.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Lares.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Lares.Controllers
{
    public class AssetController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public AssetController(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Asset
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Assets.Include(a => a.Property);
            return View(await dataContext.ToListAsync());
        }

        // GET: Asset/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets
                .Include(a => a.Property)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asset == null)
            {
                return NotFound();
            }

            return View(asset);
        }

        // GET: Asset/Create
        [Authorize]
        public IActionResult Create()
        {
            AssetViewModel assetViewModel = new AssetViewModel();

            return View(assetViewModel);
        }

        // POST: Asset/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Name,Description,Make,Model,SerialNo,AcquiredDate,Id")] AssetViewModel assetViewModel)
        {
            // get users's current property 
            var currentUser = await _userManager.GetUserAsync(this.User);
            var currentProperty = currentUser.SelectedPropertyId;

            Asset newAsset = new Asset
            {
                PropertyId = currentProperty,
                Name = assetViewModel.Name,
                Description = assetViewModel.Description,
                Make = assetViewModel.Make,
                Model = assetViewModel.Model,
                SerialNo = assetViewModel.SerialNo,
                AcquiredDate = assetViewModel.AcquiredDate
            };

            if (ModelState.IsValid)
            {
                _context.Add(newAsset);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newAsset);
        }

        // GET: Asset/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
            {
                return NotFound();
            }
            ViewData["PropertyId"] = new SelectList(_context.Property, "Id", "Id", asset.PropertyId);
            return View(asset);
        }

        // POST: Asset/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PropertyId,Name,Description,Make,Model,SerialNo,AcquiredDate,Id")] Asset asset)
        {
            if (id != asset.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asset);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssetExists(asset.Id))
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
            ViewData["PropertyId"] = new SelectList(_context.Property, "Id", "Id", asset.PropertyId);
            return View(asset);
        }

        // GET: Asset/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets
                .Include(a => a.Property)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asset == null)
            {
                return NotFound();
            }

            return View(asset);
        }

        // POST: Asset/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssetExists(int id)
        {
            return _context.Assets.Any(e => e.Id == id);
        }
    }
}
