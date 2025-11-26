using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WirtualnaUczelnia.Data;
using WirtualnaUczelnia.Models;

namespace WirtualnaUczelnia.Controllers
{
    public class LocationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public LocationsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Locations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Locations.Include(l => l.Building);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Locations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var location = await _context.Locations
                .Include(l => l.Building)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (location == null) return NotFound();

            return View(location);
        }

        // GET: Locations/Create
        public IActionResult Create()
        {
            // Tutaj "Name" zamiast "Symbol", ¿eby w liœcie by³a pe³na nazwa budynku
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Name");
            return View();
        }

        // POST: Locations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,BuildingId")] Location location, IFormFile? imageFile, IFormFile? audioFile)
        {
            // 1. Obs³uga zdjêcia
            if (imageFile != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string path = Path.Combine(wwwRootPath, "images", fileName);

                Directory.CreateDirectory(Path.Combine(wwwRootPath, "images")); // Upewnij siê, ¿e folder istnieje

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                location.ImageFileName = fileName;
            }
            else
            {
                location.ImageFileName = "default.jpg";
            }

            // 2. Obs³uga audio
            if (audioFile != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(audioFile.FileName);
                string path = Path.Combine(wwwRootPath, "audio", fileName);

                Directory.CreateDirectory(Path.Combine(wwwRootPath, "audio"));

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await audioFile.CopyToAsync(fileStream);
                }
                location.AudioFileName = fileName;
            }

            // 3. Usuwamy b³êdy walidacji dla pól, które uzupe³niliœmy rêcznie w kodzie
            // Jeœli w modelu Location te pola s¹ [Required], to bez tego ModelState.IsValid zawsze bêdzie false!
            ModelState.Remove("ImageFileName");
            ModelState.Remove("AudioFileName");
            // Jeœli masz kolekcjê Transitions w modelu i jest ona wymagana, te¿ j¹ usuñ z walidacji
            ModelState.Remove("Transitions");
            ModelState.Remove("Building"); // Relacja te¿ nie jest przesy³ana w formularzu

            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Jeœli walidacja nie przesz³a, poka¿ formularz ponownie z b³êdami
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Name", location.BuildingId);
            return View(location);
        }

        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var location = await _context.Locations.FindAsync(id);
            if (location == null) return NotFound();

            ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Name", location.BuildingId);
            return View(location);
        }

        // POST: Locations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,BuildingId")] Location location, IFormFile? imageFile, IFormFile? audioFile)
        {
            if (id != location.Id) return NotFound();

            // 1. Pobierz "star¹" wersjê z bazy, ¿eby wiedzieæ, jakie by³y nazwy plików
            // U¿ywamy AsNoTracking(), ¿eby nie blokowaæ EF przy póŸniejszym Update
            var oldLocation = await _context.Locations
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);

            if (oldLocation == null) return NotFound();

            // 2. Obs³uga Zdjêcia
            if (imageFile != null)
            {
                // U¿ytkownik wgra³ nowe zdjêcie -> Zapisz je i nadpisz nazwê
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string path = Path.Combine(wwwRootPath, "images", fileName);

                Directory.CreateDirectory(Path.Combine(wwwRootPath, "images"));

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                location.ImageFileName = fileName;
            }
            else
            {
                // U¿ytkownik NIE wgra³ zdjêcia -> Zachowaj star¹ nazwê z bazy
                location.ImageFileName = oldLocation.ImageFileName;
            }

            // 3. Obs³uga Audio
            if (audioFile != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(audioFile.FileName);
                string path = Path.Combine(wwwRootPath, "audio", fileName);

                Directory.CreateDirectory(Path.Combine(wwwRootPath, "audio"));

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await audioFile.CopyToAsync(stream);
                }
                location.AudioFileName = fileName;
            }
            else
            {
                // Zachowaj stare audio
                location.AudioFileName = oldLocation.AudioFileName;
            }

            // 4. KLUCZOWE: Usuñ walidacjê dla pól, które "naprawiliœmy" rêcznie
            ModelState.Remove("ImageFileName");
            ModelState.Remove("AudioFileName");
            ModelState.Remove("Transitions");
            ModelState.Remove("Building");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            // Jeœli tu trafiliœmy, to znaczy ¿e jest b³¹d walidacji (np. pusta Nazwa)
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Name", location.BuildingId);
            return View(location);
        }

        // GET: Locations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var location = await _context.Locations
                .Include(l => l.Building)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null) return NotFound();

            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location != null)
            {
                _context.Locations.Remove(location);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.Id == id);
        }
    }
}