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
            var buildings = await _context.Buildings.ToListAsync();
            return View(buildings);
        }

        // Akcja Wyszukiwania
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("Index");
            }

            // Szukamy w Lokacjach (Nazwa lub Opis)
            // Contains w SQL Server domyœlnie ignoruje wielkoœæ liter (Case Insensitive)
            // To proste dopasowanie - znajdzie "legitymacja" w "legitymacjami"
            var results = await _context.Locations
                .Include(l => l.Building)
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