using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WirtualnaUczelnia.Data;
using WirtualnaUczelnia.Models;
using Microsoft.AspNetCore.Authorization;

namespace WirtualnaUczelnia.Controllers
{
    public class ExploreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExploreController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> StartInBuilding(int buildingId)
        {
            // Znajdź pierwszą lokację przypisaną do tego budynku
            var startLocation = await _context.Locations
                .Include(l => l.Transitions)
                .FirstOrDefaultAsync(l => l.BuildingId == buildingId);

            if (startLocation == null)
            {
                return Content("Ten budynek nie ma jeszcze zdefiniowanych zdjęć/lokacji. Dodaj je w panelu administratora.");
            }

            // Przekieruj do widoku zwiedzania z tą lokacją
            return RedirectToAction("Index", new { id = startLocation.Id });
        }

        // Akcja domyślna - start wycieczki (np. od lokalizacji o ID 1 lub pierwszej znalezionej)
        public async Task<IActionResult> Index(int? id)
        {
            Location location;

            if (id == null)
            {
                // Jeśli nie podano ID, zacznij od pierwszej lokalizacji w bazie (np. Wejście)
                location = await _context.Locations
                    .Include(l => l.Transitions) // Załaduj możliwe przejścia
                    .FirstOrDefaultAsync();
            }
            else
            {
                // Pobierz konkretną lokalizację
                location = await _context.Locations
                    .Include(l => l.Transitions)
                    .FirstOrDefaultAsync(m => m.Id == id);
            }

            if (location == null)
            {
                return NotFound("Nie znaleziono lokalizacji startowej. Upewnij się, że baza jest zasiedlona.");
            }

            return View(location);
        }
    }
}