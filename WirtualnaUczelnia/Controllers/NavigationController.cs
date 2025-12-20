using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WirtualnaUczelnia.Data;
using WirtualnaUczelnia.Services;

namespace WirtualnaUczelnia.Controllers
{
    public class NavigationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PathFinderService _pathFinder;

        public NavigationController(ApplicationDbContext context, PathFinderService pathFinder)
        {
            _context = context;
            _pathFinder = pathFinder;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isDisabled = false;

            if (userId != null)
            {
                var pref = await _context.UserPreferences.FirstOrDefaultAsync(p => p.UserId == userId);
                if (pref != null) isDisabled = pref.IsDisabled;
            }

            ViewBag.IsDisabledUser = isDisabled;

            // Pokazuj tylko widoczne lokacje (nie ukryte i nie w ukrytych budynkach)
            var visibleLocations = await _context.Locations
                .Include(l => l.Building)
                .Where(l => !l.IsHidden)
                .Where(l => l.Building == null || !l.Building.IsHidden)
                .OrderBy(l => l.Building != null ? l.Building.Symbol : "ZZZ") // Sortuj po budynku
                .ThenBy(l => l.Name) // Potem po nazwie
                .ToListAsync();

            ViewBag.Locations = new SelectList(visibleLocations, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FindRoute(int startId, int endId)
        {
            // Walidacja: start i koniec nie mogą być takie same
            if (startId == endId)
            {
                TempData["Error"] = "Lokalizacja startowa i docelowa nie mogą być takie same.";
                return RedirectToAction(nameof(Index));
            }

            // Sprawdź czy wybrane lokacje są dostępne
            var startLoc = await _context.Locations
                .Include(l => l.Building)
                .FirstOrDefaultAsync(l => l.Id == startId);
            var endLoc = await _context.Locations
                .Include(l => l.Building)
                .FirstOrDefaultAsync(l => l.Id == endId);

            // Jeśli któraś lokacja jest ukryta, nie pozwól na wyszukanie trasy
            if (startLoc == null || startLoc.IsHidden || (startLoc.Building != null && startLoc.Building.IsHidden))
            {
                TempData["Error"] = "Lokalizacja startowa jest niedostępna.";
                return RedirectToAction(nameof(Index));
            }

            if (endLoc == null || endLoc.IsHidden || (endLoc.Building != null && endLoc.Building.IsHidden))
            {
                TempData["Error"] = "Lokalizacja docelowa jest niedostępna.";
                return RedirectToAction(nameof(Index));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var path = await _pathFinder.FindPathAsync(startId, endId, userId);

            ViewBag.StartName = startLoc?.Name;
            ViewBag.EndName = endLoc?.Name;

            return View("Result", path);
        }
    }
}