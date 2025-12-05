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
        private readonly IWebHostEnvironment _hostEnvironment; // Niezbêdne do zapisu plików

        public BuildingsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Buildings
        public async Task<IActionResult> Index()
        {
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
        public async Task<IActionResult> Create([Bind("Id,Symbol,Name,Description")] Building building, IFormFile? imageFile)
        {
            // Logika zapisu zdjêcia
            if (imageFile != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = "building_" + Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string path = Path.Combine(wwwRootPath, "images", fileName);

                // Upewnij siê, ¿e folder istnieje
                Directory.CreateDirectory(Path.Combine(wwwRootPath, "images"));

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                building.ImageFileName = fileName;
            }

            // Usuwamy ImageFileName z walidacji, bo nie jest przesy³ane w formularzu jako tekst
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Symbol,Name,Description,ImageFileName")] Building building, IFormFile? imageFile)
        {
            if (id != building.Id) return NotFound();

            // Pobieramy star¹ wersjê, ¿eby zachowaæ zdjêcie jeœli nie wybrano nowego
            var oldBuilding = await _context.Buildings.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);

            if (imageFile != null)
            {
                // U¿ytkownik wgra³ nowe zdjêcie
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
                // Zachowaj stare zdjêcie
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

        // GET: Buildings/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null) return NotFound();

            var building = await _context.Buildings
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (building == null) return NotFound();

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Nie mo¿na usun¹æ budynku, poniewa¿ s¹ do niego przypisane lokalizacje. " +
                    "Usuñ najpierw lokalizacje powi¹zane z tym budynkiem.";
            }

            return View(building);
        }

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var building = await _context.Buildings.FindAsync(id);
            if (building == null) return RedirectToAction(nameof(Index));

            try
            {
                _context.Buildings.Remove(building);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool BuildingExists(int id)
        {
            return _context.Buildings.Any(e => e.Id == id);
        }
    }
}