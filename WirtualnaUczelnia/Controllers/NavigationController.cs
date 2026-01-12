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

        // Klucz sesji dla trybu windy (dla niezalogowanych)
        private const string WheelchairModeSessionKey = "WheelchairMode";

        public NavigationController(ApplicationDbContext context, PathFinderService pathFinder)
        {
            _context = context;
            _pathFinder = pathFinder;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isDisabled = false;
            bool isLoggedIn = userId != null;

            if (isLoggedIn)
            {
                // Zalogowany użytkownik - pobierz preferencje z bazy
                var pref = await _context.UserPreferences.FirstOrDefaultAsync(p => p.UserId == userId);
                if (pref != null) isDisabled = pref.IsDisabled;
            }
            else
            {
                // Niezalogowany użytkownik - pobierz z sesji
                isDisabled = HttpContext.Session.GetString(WheelchairModeSessionKey) == "true";
            }

            ViewBag.IsDisabledUser = isDisabled;
            ViewBag.IsLoggedIn = isLoggedIn;

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
        public async Task<IActionResult> FindRoute(int startId, int endId, bool wheelchairMode = false)
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

            // Określ tryb dostępności
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool requireWheelchairAccess;

            if (userId != null)
            {
                // Zalogowany użytkownik - pobierz preferencje z bazy
                var pref = await _context.UserPreferences.FirstOrDefaultAsync(p => p.UserId == userId);
                requireWheelchairAccess = pref?.IsDisabled ?? false;
            }
            else
            {
                // Niezalogowany użytkownik - użyj wartości z formularza i zapisz w sesji
                requireWheelchairAccess = wheelchairMode;
                HttpContext.Session.SetString(WheelchairModeSessionKey, wheelchairMode ? "true" : "false");
            }

            // Znajdź trasę
            var path = await _pathFinder.FindPathAsync(startId, endId, requireWheelchairAccess);

            ViewBag.StartName = startLoc?.Name;
            ViewBag.EndName = endLoc?.Name;
            ViewBag.WheelchairMode = requireWheelchairAccess;

            return View("Result", path);
        }
    }
}