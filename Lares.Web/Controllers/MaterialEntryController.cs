using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lares.Entities;
using Lares.Infrastructure;

namespace Lares.Controllers
{
    public class MaterialEntryController : Controller
    {
        private readonly DataContext _context;

        public MaterialEntryController(DataContext context)
        {
            _context = context;
        }

        // GET: MaterialEntries
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.MaterialEntries.Include(m => m.WorkTask);
            return View(await dataContext.ToListAsync());
        }

        // GET: MaterialEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialEntry = await _context.MaterialEntries
                .Include(m => m.WorkTask)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materialEntry == null)
            {
                return NotFound();
            }

            return View(materialEntry);
        }

        // GET: MaterialEntries/Create
        public IActionResult Create()
        {
            ViewData["WorkTaskId"] = new SelectList(_context.WorkTasks, "Id", "Id");
            return View();
        }

        // POST: MaterialEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkTaskId,EntryDate,Description,QuantityUsed,CostPerUnit,Id")] MaterialEntry materialEntry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(materialEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WorkTaskId"] = new SelectList(_context.WorkTasks, "Id", "Id", materialEntry.WorkTaskId);
            return View(materialEntry);
        }

        // GET: MaterialEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialEntry = await _context.MaterialEntries.FindAsync(id);
            if (materialEntry == null)
            {
                return NotFound();
            }
            ViewData["WorkTaskId"] = new SelectList(_context.WorkTasks, "Id", "Id", materialEntry.WorkTaskId);
            return View(materialEntry);
        }

        // POST: MaterialEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkTaskId,EntryDate,Description,QuantityUsed,CostPerUnit,Id")] MaterialEntry materialEntry)
        {
            if (id != materialEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materialEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialEntryExists(materialEntry.Id))
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
            ViewData["WorkTaskId"] = new SelectList(_context.WorkTasks, "Id", "Id", materialEntry.WorkTaskId);
            return View(materialEntry);
        }

        // GET: MaterialEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialEntry = await _context.MaterialEntries
                .Include(m => m.WorkTask)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materialEntry == null)
            {
                return NotFound();
            }

            return View(materialEntry);
        }

        // POST: MaterialEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materialEntry = await _context.MaterialEntries.FindAsync(id);
            _context.MaterialEntries.Remove(materialEntry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialEntryExists(int id)
        {
            return _context.MaterialEntries.Any(e => e.Id == id);
        }
    }
}
