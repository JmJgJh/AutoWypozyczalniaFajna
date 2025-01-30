using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoWypozyczalniaFajna.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AutoWypozyczalniaFajna.Controllers
{
    [Authorize]
    public class WypozyczeniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WypozyczeniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Wypozyczenies
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Pobierz ID aktualnego użytkownika
            var wypozyczenia = await _context.Wypozyczenie
                .Include(w => w.Samochod)
                .Where(w => w.UserId == userId) // Tylko wypożyczenia tego użytkownika
                .ToListAsync();

            foreach (var wypozyczenie in wypozyczenia)
            {
                if (wypozyczenie.DataZwrotu < DateTime.Now)
                {
                    var samochod = await _context.Samochod.FindAsync(wypozyczenie.SamochodId);
                    if (samochod != null)
                    {
                        samochod.IsAvailable = true;
                        _context.Update(samochod);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return View(wypozyczenia);
        }

        // GET: Wypozyczenies/Create
        public IActionResult Create()
        {
            var availableCars = _context.Samochod
                .Where(s => s.IsAvailable && !_context.Wypozyczenie
                    .Any(w => w.SamochodId == s.Id &&
                              (w.DataWypozyczenia < DateTime.Now && w.DataZwrotu > DateTime.Now)))
                .ToList();

            ViewData["SamochodId"] = new SelectList(availableCars, "Id", "Model");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");

            // Przekazanie cen samochodów
            ViewBag.CenySamochodow = availableCars.ToDictionary(s => s.Id, s => s.CenaZaDzien);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SamochodId,DataWypozyczenia,DataZwrotu,UserId")] Wypozyczenie wypozyczenie)
        {
            if (ModelState.IsValid)
            {
                var samochod = await _context.Samochod.FindAsync(wypozyczenie.SamochodId);
                if (samochod != null)
                {
                    int liczbaDni = (wypozyczenie.DataZwrotu - wypozyczenie.DataWypozyczenia).Days;
                    wypozyczenie.CenaCalkowita = liczbaDni * samochod.CenaZaDzien;

                    samochod.IsAvailable = false;
                    _context.Update(samochod);
                }

                wypozyczenie.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Przypisanie aktualnego użytkownika
                _context.Add(wypozyczenie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["SamochodId"] = new SelectList(_context.Samochod, "Id", "Model", wypozyczenie.SamochodId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", wypozyczenie.UserId);
            ViewBag.CenySamochodow = _context.Samochod.ToDictionary(s => s.Id, s => s.CenaZaDzien);

            return View(wypozyczenie);
        }

        // GET: Wypozyczenies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wypozyczenie = await _context.Wypozyczenie.FindAsync(id);
            if (wypozyczenie == null || wypozyczenie.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier)) // Sprawdzanie, czy to wypożyczenie użytkownika
            {
                return Forbid(); // Blokowanie dostępu do wypożyczeń innych użytkowników
            }

            var availableCars = _context.Samochod.ToList();

            ViewData["SamochodId"] = new SelectList(availableCars, "Id", "Model", wypozyczenie.SamochodId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", wypozyczenie.UserId);

            // Przekazanie cen samochodów
            ViewBag.CenySamochodow = availableCars.ToDictionary(s => s.Id, s => s.CenaZaDzien);

            return View(wypozyczenie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SamochodId,DataWypozyczenia,DataZwrotu,UserId")] Wypozyczenie wypozyczenie)
        {
            if (id != wypozyczenie.Id)
            {
                return NotFound();
            }

            if (wypozyczenie.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier)) // Sprawdzanie, czy to wypożyczenie użytkownika
            {
                return Forbid(); // Blokowanie dostępu do wypożyczeń innych użytkowników
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var samochod = await _context.Samochod.FindAsync(wypozyczenie.SamochodId);
                    if (samochod != null)
                    {
                        int liczbaDni = (wypozyczenie.DataZwrotu - wypozyczenie.DataWypozyczenia).Days;
                        wypozyczenie.CenaCalkowita = liczbaDni * samochod.CenaZaDzien;
                    }

                    _context.Update(wypozyczenie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WypozyczenieExists(wypozyczenie.Id))
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

            ViewData["SamochodId"] = new SelectList(_context.Samochod, "Id", "Model", wypozyczenie.SamochodId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", wypozyczenie.UserId);
            ViewBag.CenySamochodow = _context.Samochod.ToDictionary(s => s.Id, s => s.CenaZaDzien);

            return View(wypozyczenie);
        }

        // GET: Wypozyczenies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wypozyczenie = await _context.Wypozyczenie
                .Include(w => w.Samochod) // Wczytanie danych o samochodzie
                .Include(w => w.User)     // Wczytanie danych o użytkowniku
                .FirstOrDefaultAsync(m => m.Id == id);

            if (wypozyczenie == null || wypozyczenie.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier)) // Sprawdzanie, czy wypożyczenie należy do aktualnego użytkownika
            {
                return Forbid(); // Blokowanie dostępu do wypożyczenia innego użytkownika
            }

            return View(wypozyczenie); // Zwracamy widok z danymi wypożyczenia
        }




        // POST: Wypozyczenies/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, [Bind("Id,SamochodId,DataWypozyczenia,DataZwrotu,UserId,CenaCalkowita")] Wypozyczenie wypozyczenie)
        {
            if (id != wypozyczenie.Id)
            {
                return NotFound();
            }

            if (wypozyczenie.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier)) // Sprawdzanie, czy to wypożyczenie użytkownika
            {
                return Forbid(); // Blokowanie dostępu do wypożyczeń innych użytkowników
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var samochod = await _context.Samochod.FindAsync(wypozyczenie.SamochodId);
                    if (samochod != null)
                    {
                        // Obliczenie nowej ceny całkowitej, jeśli zmieniona została data zwrotu
                        int liczbaDni = (wypozyczenie.DataZwrotu - wypozyczenie.DataWypozyczenia).Days;
                        wypozyczenie.CenaCalkowita = liczbaDni * samochod.CenaZaDzien;
                    }

                    _context.Update(wypozyczenie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WypozyczenieExists(wypozyczenie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index)); // Po zapisaniu, przekierowanie do widoku indeksu
            }

            // W razie błędów walidacji, przekazanie z powrotem danych i ponowne wyświetlenie widoku
            ViewData["SamochodId"] = new SelectList(_context.Samochod, "Id", "Model", wypozyczenie.SamochodId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", wypozyczenie.UserId);
            return View(wypozyczenie);
        }

        // GET: Wypozyczenies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wypozyczenie = await _context.Wypozyczenie
                .Include(w => w.Samochod)
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wypozyczenie == null || wypozyczenie.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier)) // Sprawdzanie, czy to wypożyczenie użytkownika
            {
                return Forbid(); // Blokowanie dostępu do wypożyczeń innych użytkowników
            }

            return View(wypozyczenie);
        }

        // POST: Wypozyczenies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wypozyczenie = await _context.Wypozyczenie.FindAsync(id);
            if (wypozyczenie != null && wypozyczenie.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)) // Sprawdzanie, czy to wypożyczenie użytkownika
            {
                var samochod = await _context.Samochod.FindAsync(wypozyczenie.SamochodId);
                if (samochod != null)
                {
                    samochod.IsAvailable = true;
                    _context.Update(samochod);
                }

                _context.Wypozyczenie.Remove(wypozyczenie);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool WypozyczenieExists(int id)
        {
            return _context.Wypozyczenie.Any(e => e.Id == id);
        }
    }
}
