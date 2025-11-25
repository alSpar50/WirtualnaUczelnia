using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WirtualnaUczelnia.Data;
using WirtualnaUczelnia.Models;

namespace WirtualnaUczelnia.Controllers
{
    public class ExploreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExploreController(ApplicationDbContext context)
        {
            _context = context;
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