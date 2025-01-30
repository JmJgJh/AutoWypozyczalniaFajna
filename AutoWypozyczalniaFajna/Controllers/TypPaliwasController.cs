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
    [Authorize(Roles = "Administrator")]
    public class TypPaliwasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TypPaliwasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TypPaliwas
        public async Task<IActionResult> Index()
        {
            return View(await _context.TypPaliwa.ToListAsync());
        }

        // GET: TypPaliwas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typPaliwa = await _context.TypPaliwa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typPaliwa == null)
            {
                return NotFound();
            }

            return View(typPaliwa);
        }

        // GET: TypPaliwas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypPaliwas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Paliwo")] TypPaliwa typPaliwa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typPaliwa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typPaliwa);
        }

        // GET: TypPaliwas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typPaliwa = await _context.TypPaliwa.FindAsync(id);
            if (typPaliwa == null)
            {
                return NotFound();
            }
            return View(typPaliwa);
        }

        // POST: TypPaliwas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Paliwo")] TypPaliwa typPaliwa)
        {
            if (id != typPaliwa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typPaliwa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypPaliwaExists(typPaliwa.Id))
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
            return View(typPaliwa);
        }

        // GET: TypPaliwas/Delete/5

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typPaliwa = await _context.TypPaliwa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typPaliwa == null)
            {
                return NotFound();
            }

            return View(typPaliwa);
        }

        // POST: TypPaliwas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var typPaliwa = await _context.TypPaliwa.FindAsync(id);
            if (typPaliwa != null)
            {
                _context.TypPaliwa.Remove(typPaliwa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypPaliwaExists(int id)
        {
            return _context.TypPaliwa.Any(e => e.Id == id);
        }
    }
}
