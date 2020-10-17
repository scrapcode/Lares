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
    public class TimeEntryController : Controller
    {
        private readonly DataContext _context;

        public TimeEntryController(DataContext context)
        {
            _context = context;
        }

        // GET: TimeEntries
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.TimeEntries.Include(t => t.Resource).Include(t => t.WorkTask);
            return View(await dataContext.ToListAsync());
        }

        // GET: TimeEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeEntry = await _context.TimeEntries
                .Include(t => t.Resource)
                .Include(t => t.WorkTask)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeEntry == null)
            {
                return NotFound();
            }

            return View(timeEntry);
        }

        // GET: TimeEntries/Create
        public IActionResult Create()
        {
            ViewData["ResourceId"] = new SelectList(_context.Resources, "Id", "Id");
            ViewData["WorkTaskId"] = new SelectList(_context.WorkTasks, "Id", "Id");
            return View();
        }

        // POST: TimeEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkTaskId,ResourceId,DateOfWork,HoursWorked,Description,Id")] TimeEntry timeEntry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(timeEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ResourceId"] = new SelectList(_context.Resources, "Id", "Id", timeEntry.ResourceId);
            ViewData["WorkTaskId"] = new SelectList(_context.WorkTasks, "Id", "Id", timeEntry.WorkTaskId);
            return View(timeEntry);
        }

        // GET: TimeEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeEntry = await _context.TimeEntries.FindAsync(id);
            if (timeEntry == null)
            {
                return NotFound();
            }
            ViewData["ResourceId"] = new SelectList(_context.Resources, "Id", "Id", timeEntry.ResourceId);
            ViewData["WorkTaskId"] = new SelectList(_context.WorkTasks, "Id", "Id", timeEntry.WorkTaskId);
            return View(timeEntry);
        }

        // POST: TimeEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkTaskId,ResourceId,DateOfWork,HoursWorked,Description,Id")] TimeEntry timeEntry)
        {
            if (id != timeEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeEntryExists(timeEntry.Id))
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
            ViewData["ResourceId"] = new SelectList(_context.Resources, "Id", "Id", timeEntry.ResourceId);
            ViewData["WorkTaskId"] = new SelectList(_context.WorkTasks, "Id", "Id", timeEntry.WorkTaskId);
            return View(timeEntry);
        }

        // GET: TimeEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeEntry = await _context.TimeEntries
                .Include(t => t.Resource)
                .Include(t => t.WorkTask)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeEntry == null)
            {
                return NotFound();
            }

            return View(timeEntry);
        }

        // POST: TimeEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timeEntry = await _context.TimeEntries.FindAsync(id);
            _context.TimeEntries.Remove(timeEntry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimeEntryExists(int id)
        {
            return _context.TimeEntries.Any(e => e.Id == id);
        }
    }
}
