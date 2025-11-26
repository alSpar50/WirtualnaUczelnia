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
            // Pobieramy listê wszystkich budynków z bazy
            var buildings = await _context.Buildings.ToListAsync();

            // Przekazujemy listê budynków do widoku (zgodnie z @model IEnumerable<Building>)
            return View(buildings);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}