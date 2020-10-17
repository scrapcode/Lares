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
    public class WorkTaskController : Controller
    {
        private readonly DataContext _context;

        public WorkTaskController(DataContext context)
        {
            _context = context;
        }

        // GET: WorkTask
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.WorkTasks.Include(w => w.Asset);
            return View(await dataContext.ToListAsync());
        }

        // GET: WorkTask/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workTask = await _context.WorkTasks
                .Include(w => w.Asset)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workTask == null)
            {
                return NotFound();
            }

            return View(workTask);
        }

        // GET: WorkTask/Create
        public IActionResult Create()
        {
            ViewData["AssetId"] = new SelectList(_context.Assets, "Id", "Id");
            return View();
        }

        // POST: WorkTask/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AssetId,Title,Description,Status,Id")] WorkTask workTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssetId"] = new SelectList(_context.Assets, "Id", "Id", workTask.AssetId);
            return View(workTask);
        }

        // GET: WorkTask/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workTask = await _context.WorkTasks.FindAsync(id);
            if (workTask == null)
            {
                return NotFound();
            }
            ViewData["AssetId"] = new SelectList(_context.Assets, "Id", "Id", workTask.AssetId);
            return View(workTask);
        }

        // POST: WorkTask/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AssetId,Title,Description,Status,Id")] WorkTask workTask)
        {
            if (id != workTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkTaskExists(workTask.Id))
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
            ViewData["AssetId"] = new SelectList(_context.Assets, "Id", "Id", workTask.AssetId);
            return View(workTask);
        }

        // GET: WorkTask/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workTask = await _context.WorkTasks
                .Include(w => w.Asset)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workTask == null)
            {
                return NotFound();
            }

            return View(workTask);
        }

        // POST: WorkTask/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workTask = await _context.WorkTasks.FindAsync(id);
            _context.WorkTasks.Remove(workTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkTaskExists(int id)
        {
            return _context.WorkTasks.Any(e => e.Id == id);
        }
    }
}
