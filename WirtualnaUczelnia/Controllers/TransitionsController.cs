using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WirtualnaUczelnia.Data;
using WirtualnaUczelnia.Models;

namespace WirtualnaUczelnia.Controllers
{
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
        public async Task<IActionResult> Create([Bind("Id,Direction,PositionX,PositionY,SourceLocationId,TargetLocationId")] Transition transition)
        {
            // Usuwamy walidacjê obiektów nawigacyjnych, bo formularz przesy³a tylko ID
            ModelState.Remove("SourceLocation");
            ModelState.Remove("TargetLocation");

            if (ModelState.IsValid)
            {
                _context.Add(transition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // W razie b³êdu przywracamy dane do widoku
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Direction,PositionX,PositionY,SourceLocationId,TargetLocationId")] Transition transition)
        {
            if (id != transition.Id) return NotFound();

            ModelState.Remove("SourceLocation");
            ModelState.Remove("TargetLocation");

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
            return View(transition);
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