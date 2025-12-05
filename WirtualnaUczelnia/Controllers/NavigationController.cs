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

            // Diagnostyka: Sprawdźmy, czy system widzi użytkownika jako niepełnosprawnego
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isDisabled = false;

            if (userId != null)
            {
                var pref = await _context.UserPreferences.FirstOrDefaultAsync(p => p.UserId == userId);
                if (pref != null) isDisabled = pref.IsDisabled;
            }

            ViewBag.IsDisabledUser = isDisabled; // Przekazujemy status do widoku
            ViewBag.Locations = new SelectList(_context.Locations, "Id", "Name");


            // Lista lokacji do wyboru
            ViewBag.Locations = new SelectList(_context.Locations, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FindRoute(int startId, int endId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Pobierz ID zalogowanego usera

            var path = await _pathFinder.FindPathAsync(startId, endId, userId);

            // Pobieramy nazwy dla nagłówka
            var startLoc = await _context.Locations.FindAsync(startId);
            var endLoc = await _context.Locations.FindAsync(endId);

            ViewBag.StartName = startLoc?.Name;
            ViewBag.EndName = endLoc?.Name;

            return View("Result", path);
        }
    }
}