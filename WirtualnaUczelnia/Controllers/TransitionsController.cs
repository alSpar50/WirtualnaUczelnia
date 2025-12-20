using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WirtualnaUczelnia.Data;
using WirtualnaUczelnia.Models;
using Microsoft.AspNetCore.Authorization;

namespace WirtualnaUczelnia.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TransitionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransitionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transitions
        public async Task<IActionResult> Index()
        {
            var transitions = _context.Transitions
                .Include(t => t.SourceLocation)
                .Include(t => t.TargetLocation);
            return View(await transitions.ToListAsync());
        }

        // GET: Transitions/Create
        public IActionResult Create()
        {
            // Pobieramy listê lokacji, ale potrzebujemy te¿ nazw plików zdjêæ do podgl¹du!
            // Tworzymy obiekt anonimowy z ID, Nazw¹ i Plikiem
            var locations = _context.Locations
                .Select(l => new { l.Id, l.Name, l.ImageFileName })
                .ToList();

            ViewData["SourceLocationId"] = new SelectList(locations, "Id", "Name");
            ViewData["TargetLocationId"] = new SelectList(locations, "Id", "Name");

            // Przekazujemy mapê (S³ownik) ID -> NazwaPliku do widoku, aby JavaScript móg³ podmieniaæ zdjêcia
            // Serializujemy to do JSON, ¿eby JS móg³ to ³atwo odczytaæ
            var imageMap = locations.ToDictionary(k => k.Id.ToString(), v => v.ImageFileName);
            ViewBag.ImageMap = System.Text.Json.JsonSerializer.Serialize(imageMap);

            return View();
        }

        // POST: Transitions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // DODANO: "IsWheelchairAccessible" do listy Bind
        public async Task<IActionResult> Create([Bind("Id,Direction,PositionX,PositionY,SourceLocationId,TargetLocationId,IsWheelchairAccessible,IsHidden,Cost")] Transition transition)
        {
            ModelState.Remove("SourceLocation");
            ModelState.Remove("TargetLocation");

            // Ustaw domyœlny koszt jeœli nie podano
            if (transition.Cost <= 0)
                transition.Cost = 10;

            if (ModelState.IsValid)
            {
                _context.Add(transition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Przywracanie danych do widoku w razie b³êdu
            var locations = _context.Locations.Select(l => new { l.Id, l.Name, l.ImageFileName }).ToList();
            ViewData["SourceLocationId"] = new SelectList(locations, "Id", "Name", transition.SourceLocationId);
            ViewData["TargetLocationId"] = new SelectList(locations, "Id", "Name", transition.TargetLocationId);

            var imageMap = locations.ToDictionary(k => k.Id.ToString(), v => v.ImageFileName);
            ViewBag.ImageMap = System.Text.Json.JsonSerializer.Serialize(imageMap);

            return View(transition);
        }

        // GET: Transitions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var transition = await _context.Transitions.FindAsync(id);
            if (transition == null) return NotFound();

            var locations = _context.Locations.Select(l => new { l.Id, l.Name, l.ImageFileName }).ToList();
            ViewData["SourceLocationId"] = new SelectList(locations, "Id", "Name", transition.SourceLocationId);
            ViewData["TargetLocationId"] = new SelectList(locations, "Id", "Name", transition.TargetLocationId);

            var imageMap = locations.ToDictionary(k => k.Id.ToString(), v => v.ImageFileName);
            ViewBag.ImageMap = System.Text.Json.JsonSerializer.Serialize(imageMap);

            return View(transition);
        }

        // POST: Transitions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // DODANO: "IsWheelchairAccessible" do listy Bind
        public async Task<IActionResult> Edit(int id, [Bind("Id,Direction,PositionX,PositionY,SourceLocationId,TargetLocationId,IsWheelchairAccessible,IsHidden,Cost")] Transition transition)
        {
            if (id != transition.Id) return NotFound();

            ModelState.Remove("SourceLocation");
            ModelState.Remove("TargetLocation");

            // Ustaw minimalny koszt
            if (transition.Cost <= 0)
                transition.Cost = 10;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransitionExists(transition.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var locations = _context.Locations.Select(l => new { l.Id, l.Name, l.ImageFileName }).ToList();
            ViewData["SourceLocationId"] = new SelectList(locations, "Id", "Name", transition.SourceLocationId);
            ViewData["TargetLocationId"] = new SelectList(locations, "Id", "Name", transition.TargetLocationId);

            var imageMap = locations.ToDictionary(k => k.Id.ToString(), v => v.ImageFileName);
            ViewBag.ImageMap = System.Text.Json.JsonSerializer.Serialize(imageMap);

            return View(transition);
        }

        // POST: Transitions/ToggleVisibility/5 - Nowa akcja do prze³¹czania widocznoœci
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleVisibility(int id)
        {
            var transition = await _context.Transitions
                .Include(t => t.SourceLocation)
                .Include(t => t.TargetLocation)
                .FirstOrDefaultAsync(t => t.Id == id);
                
            if (transition == null) return NotFound();

            transition.IsHidden = !transition.IsHidden;
            await _context.SaveChangesAsync();

            string sourceName = transition.SourceLocation?.Name ?? "?";
            string targetName = transition.TargetLocation?.Name ?? "?";
            
            TempData["Message"] = transition.IsHidden 
                ? $"Przejœcie z \"{sourceName}\" do \"{targetName}\" zosta³o ukryte." 
                : $"Przejœcie z \"{sourceName}\" do \"{targetName}\" jest teraz widoczne.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Transitions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var transition = await _context.Transitions
                .Include(t => t.SourceLocation)
                .Include(t => t.TargetLocation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transition == null) return NotFound();

            return View(transition);
        }

        // POST: Transitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transition = await _context.Transitions.FindAsync(id);
            if (transition != null) _context.Transitions.Remove(transition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransitionExists(int id)
        {
            return _context.Transitions.Any(e => e.Id == id);
        }
    }
}