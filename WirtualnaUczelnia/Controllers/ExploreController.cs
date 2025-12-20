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
            // Sprawdź czy budynek nie jest ukryty
            var building = await _context.Buildings.FindAsync(buildingId);
            if (building == null || building.IsHidden)
            {
                return NotFound("Budynek nie został znaleziony lub jest obecnie niedostępny.");
            }

            // Znajdź pierwszą widoczną lokację przypisaną do tego budynku
            var startLocation = await _context.Locations
                .Include(l => l.Transitions)
                .Where(l => l.BuildingId == buildingId && !l.IsHidden)
                .FirstOrDefaultAsync();

            if (startLocation == null)
            {
                return Content("Ten budynek nie ma jeszcze zdefiniowanych zdjęć/lokacji lub wszystkie są ukryte. Dodaj je w panelu administratora.");
            }

            return RedirectToAction("Index", new { id = startLocation.Id });
        }

        // Akcja domyślna - start wycieczki
        public async Task<IActionResult> Index(int? id)
        {
            Location location;

            if (id == null)
            {
                // Jeśli nie podano ID, zacznij od pierwszej widocznej lokalizacji
                location = await _context.Locations
                    .Include(l => l.Transitions)
                        .ThenInclude(t => t.TargetLocation)
                    .Where(l => !l.IsHidden)
                    .Where(l => l.Building == null || !l.Building.IsHidden)
                    .FirstOrDefaultAsync();
            }
            else
            {
                // Pobierz konkretną lokalizację
                location = await _context.Locations
                    .Include(l => l.Building)
                    .Include(l => l.Transitions)
                        .ThenInclude(t => t.TargetLocation)
                    .FirstOrDefaultAsync(m => m.Id == id);

                // Sprawdź czy lokacja lub jej budynek nie są ukryte
                if (location != null)
                {
                    if (location.IsHidden || (location.Building != null && location.Building.IsHidden))
                    {
                        return NotFound("Ta lokalizacja jest obecnie niedostępna.");
                    }
                }
            }

            if (location == null)
            {
                return NotFound("Nie znaleziono lokalizacji. Upewnij się, że istnieją widoczne lokalizacje w bazie.");
            }

            // Filtruj przejścia - pokaż tylko te, które:
            // 1. Nie są ukryte
            // 2. Prowadzą do widocznych lokalizacji
            // 3. Prowadzą do lokalizacji w widocznych budynkach
            if (location.Transitions != null)
            {
                location.Transitions = location.Transitions
                    .Where(t => !t.IsHidden)
                    .Where(t => t.TargetLocation != null && !t.TargetLocation.IsHidden)
                    .Where(t => t.TargetLocation.Building == null || !t.TargetLocation.Building.IsHidden)
                    .ToList();
            }

            return View(location);
        }
    }
}