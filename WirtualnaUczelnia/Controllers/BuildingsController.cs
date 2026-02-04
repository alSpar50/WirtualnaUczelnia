using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WirtualnaUczelnia.Data;
using WirtualnaUczelnia.Models;

namespace WirtualnaUczelnia.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BuildingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BuildingsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Buildings
        public async Task<IActionResult> Index()
        {
            // Admin widzi wszystkie budynki (równie¿ ukryte)
            return View(await _context.Buildings.ToListAsync());
        }

        // GET: Buildings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var building = await _context.Buildings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (building == null) return NotFound();

            return View(building);
        }

        // GET: Buildings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Buildings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Symbol,Name,Description,ImageAltText,IsHidden")] Building building, IFormFile? imageFile)
        {
            if (imageFile != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = "building_" + Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string path = Path.Combine(wwwRootPath, "images", fileName);

                Directory.CreateDirectory(Path.Combine(wwwRootPath, "images"));

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                building.ImageFileName = fileName;
            }

            ModelState.Remove("ImageFileName");
            ModelState.Remove("Locations");

            if (ModelState.IsValid)
            {
                _context.Add(building);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(building);
        }

        // GET: Buildings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var building = await _context.Buildings.FindAsync(id);
            if (building == null) return NotFound();
            return View(building);
        }

        // POST: Buildings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Symbol,Name,Description,ImageFileName,ImageAltText,IsHidden")] Building building, IFormFile? imageFile)
        {
            if (id != building.Id) return NotFound();

            var oldBuilding = await _context.Buildings.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);

            if (imageFile != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = "building_" + Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string path = Path.Combine(wwwRootPath, "images", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                building.ImageFileName = fileName;
            }
            else
            {
                building.ImageFileName = oldBuilding?.ImageFileName;
            }

            ModelState.Remove("ImageFileName");
            ModelState.Remove("Locations");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(building);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuildingExists(building.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(building);
        }

        // POST: Buildings/ToggleVisibility/5 - Nowa akcja do prze³¹czania widocznoœci
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleVisibility(int id)
        {
            var building = await _context.Buildings.FindAsync(id);
            if (building == null) return NotFound();

            building.IsHidden = !building.IsHidden;
            await _context.SaveChangesAsync();

            TempData["Message"] = building.IsHidden 
                ? $"Budynek {building.Symbol} zosta³ ukryty." 
                : $"Budynek {building.Symbol} jest teraz widoczny.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Buildings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var building = await _context.Buildings
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (building == null) return NotFound();

            // Pobierz powi¹zane lokacje
            var relatedLocations = await _context.Locations
                .Where(l => l.BuildingId == id)
                .ToListAsync();

            // Pobierz powi¹zane przejœcia (przez lokacje)
            var locationIds = relatedLocations.Select(l => l.Id).ToList();
            var relatedTransitions = await _context.Transitions
                .Include(t => t.SourceLocation)
                .Include(t => t.TargetLocation)
                .Where(t => locationIds.Contains(t.SourceLocationId) || locationIds.Contains(t.TargetLocationId))
                .ToListAsync();

            ViewBag.RelatedLocations = relatedLocations;
            ViewBag.RelatedTransitions = relatedTransitions;
            ViewBag.HasRelatedData = relatedLocations.Any() || relatedTransitions.Any();

            return View(building);
        }

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var building = await _context.Buildings.FindAsync(id);
            if (building == null) return RedirectToAction(nameof(Index));

            // Pobierz powi¹zane lokacje
            var relatedLocations = await _context.Locations
                .Where(l => l.BuildingId == id)
                .ToListAsync();

            var locationIds = relatedLocations.Select(l => l.Id).ToList();

            // Pobierz i usuñ powi¹zane przejœcia
            var relatedTransitions = await _context.Transitions
                .Where(t => locationIds.Contains(t.SourceLocationId) || locationIds.Contains(t.TargetLocationId))
                .ToListAsync();

            int transitionsCount = relatedTransitions.Count;
            int locationsCount = relatedLocations.Count;

            if (relatedTransitions.Any())
            {
                _context.Transitions.RemoveRange(relatedTransitions);
            }

            // Usuñ powi¹zane lokacje
            if (relatedLocations.Any())
            {
                _context.Locations.RemoveRange(relatedLocations);
            }

            // Usuñ budynek
            _context.Buildings.Remove(building);
            await _context.SaveChangesAsync();

            if (transitionsCount > 0 || locationsCount > 0)
            {
                TempData["Message"] = $"Budynek \"{building.Name}\" zosta³ usuniêty wraz z {locationsCount} lokacjami i {transitionsCount} przejœciami.";
            }
            else
            {
                TempData["Message"] = $"Budynek \"{building.Name}\" zosta³ usuniêty.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BuildingExists(int id)
        {
            return _context.Buildings.Any(e => e.Id == id);
        }
    }
}