using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoWypozyczalniaFajna.Data;
using AutoWypozyczalniaFajna.Models;
using Microsoft.AspNetCore.Authorization;

namespace AutoWypozyczalniaFajna.Controllers
{
    public class SamochodsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SamochodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Samochods
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Samochod.Include(s => s.Marka).Include(s => s.TypPaliwa);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Samochods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var samochod = await _context.Samochod
                .Include(s => s.Marka)
                .Include(s => s.TypPaliwa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (samochod == null)
            {
                return NotFound();
            }

            return View(samochod);
        }

        // GET: Samochods/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["MarkaId"] = new SelectList(_context.Marka, "Id", "MarkaNazwa");
            ViewData["TypPaliwaId"] = new SelectList(_context.TypPaliwa, "Id", "Paliwo");
            return View();
        }

        // POST: Samochods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,MarkaId,Model,TypPaliwaId,NrRejestracyjny,CenaZaDzien")] Samochod samochod)
        {
            if (ModelState.IsValid)
            {
                _context.Add(samochod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MarkaId"] = new SelectList(_context.Marka, "Id", "MarkaNazwa", samochod.MarkaId);
            ViewData["TypPaliwaId"] = new SelectList(_context.TypPaliwa, "Id", "Paliwo", samochod.TypPaliwaId);
            return View(samochod);
        }

        // GET: Samochods/Edit/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var samochod = await _context.Samochod.FindAsync(id);
            if (samochod == null)
            {
                return NotFound();
            }
            ViewData["MarkaId"] = new SelectList(_context.Marka, "Id", "MarkaNazwa", samochod.MarkaId);
            ViewData["TypPaliwaId"] = new SelectList(_context.TypPaliwa, "Id", "Paliwo", samochod.TypPaliwaId);
            return View(samochod);
        }

        // POST: Samochods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MarkaId,Model,TypPaliwaId,NrRejestracyjny,CenaZaDzien")] Samochod samochod)
        {
            if (id != samochod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(samochod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SamochodExists(samochod.Id))
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
            ViewData["MarkaId"] = new SelectList(_context.Marka, "Id", "MarkaNazwa", samochod.MarkaId);
            ViewData["TypPaliwaId"] = new SelectList(_context.TypPaliwa, "Id", "Paliwo", samochod.TypPaliwaId);
            return View(samochod);
        }

        // GET: Samochods/Delete/5
           [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var samochod = await _context.Samochod
                .Include(s => s.Marka)
                .Include(s => s.TypPaliwa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (samochod == null)
            {
                return NotFound();
            }

            return View(samochod);
        }

        // POST: Samochods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var samochod = await _context.Samochod.FindAsync(id);
            if (samochod != null)
            {
                _context.Samochod.Remove(samochod);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SamochodExists(int id)
        {
            return _context.Samochod.Any(e => e.Id == id);
        }
    }
}
