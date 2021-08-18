using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SSW.Med_Man.MVC.Data;
using SSW.Med_Man.MVC.Models;

namespace SSW.Med_Man.MVC.Controllers
{
    public class AdministrationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdministrationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Administrations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Administrations.Include(a => a.medication).Include(a => a.patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Administrations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrations = await _context.Administrations
                .Include(a => a.medication)
                .Include(a => a.patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administrations == null)
            {
                return NotFound();
            }

            return View(administrations);
        }

        // GET: Administrations/Create
        public IActionResult Create()
        {
            ViewData["medicationId"] = new SelectList(_context.Medications, "Id", "name");
            ViewData["patientId"] = new SelectList(_context.Patients, "Id", "FullName");
            ViewData["timeGiven"] = DateTime.Now;
            return View();
        }

        // POST: Administrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("patientId,medicationId,dose,timeGiven,Id")] Administrations administrations)
        {
            if (ModelState.IsValid)
            {
                _context.Add(administrations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["medicationId"] = new SelectList(_context.Medications, "Id", "name", administrations.medicationId);
            ViewData["patientId"] = new SelectList(_context.Patients, "Id", "FullName", administrations.patientId);
            return View(administrations);
        }

        // GET: Administrations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrations = await _context.Administrations.FindAsync(id);
            if (administrations == null)
            {
                return NotFound();
            }
            ViewData["medicationId"] = new SelectList(_context.Medications, "Id", "name", administrations.medicationId);
            ViewData["patientId"] = new SelectList(_context.Patients, "Id", "FullName", administrations.patientId);
            return View(administrations);
        }

        // POST: Administrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("patientId,medicationId,dose,timeGiven,Id")] Administrations administrations)
        {
            if (id != administrations.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(administrations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdministrationsExists(administrations.Id))
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
            ViewData["medicationId"] = new SelectList(_context.Medications, "Id", "name", administrations.medicationId);
            ViewData["patientId"] = new SelectList(_context.Patients, "Id", "FullName", administrations.patientId);
            return View(administrations);
        }

        // GET: Administrations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrations = await _context.Administrations
                .Include(a => a.medication)
                .Include(a => a.patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administrations == null)
            {
                return NotFound();
            }

            return View(administrations);
        }

        // POST: Administrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var administrations = await _context.Administrations.FindAsync(id);
            _context.Administrations.Remove(administrations);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdministrationsExists(int id)
        {
            return _context.Administrations.Any(e => e.Id == id);
        }
    }
}
