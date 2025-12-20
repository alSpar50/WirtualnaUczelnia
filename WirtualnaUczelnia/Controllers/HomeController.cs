using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WirtualnaUczelnia.Data;
using WirtualnaUczelnia.Models;

namespace WirtualnaUczelnia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Filtruj ukryte budynki - u¿ytkownicy ich nie widz¹
            var buildings = await _context.Buildings
                .Where(b => !b.IsHidden)
                .ToListAsync();
            return View(buildings);
        }

        // Akcja Wyszukiwania
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("Index");
            }

            // Szukamy tylko w widocznych lokacjach (nie ukrytych)
            // oraz tylko w widocznych budynkach
            var results = await _context.Locations
                .Include(l => l.Building)
                .Where(l => !l.IsHidden) // Ukryte lokacje nie s¹ wyœwietlane
                .Where(l => l.Building == null || !l.Building.IsHidden) // Lokacje z ukrytych budynków te¿ nie
                .Where(l => l.Name.Contains(query) || (l.Description != null && l.Description.Contains(query)))
                .ToListAsync();

            ViewBag.Query = query;
            return View(results);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}