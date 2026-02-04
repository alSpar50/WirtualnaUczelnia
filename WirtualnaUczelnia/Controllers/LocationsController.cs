using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WirtualnaUczelnia.Data;
using WirtualnaUczelnia.Models;
using Microsoft.AspNetCore.Authorization;

namespace WirtualnaUczelnia.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LocationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        
        // Maksymalny rozmiar pliku: 10 MB
        private const long MaxFileSize = 10 * 1024 * 1024;
        // Dozwolone rozszerzenia obrazów
        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        // Dozwolone rozszerzenia audio
        private static readonly string[] AllowedAudioExtensions = { ".mp3", ".wav", ".ogg", ".m4a" };

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
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Name");
            return View();
        }

        // POST: Locations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(52428800)]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,BuildingId,ImageAltText,IsHidden,Type,Floor")] Location location, IFormFile? imageFile, IFormFile? audioFile)
        {
            // Walidacja pliku obrazu
            if (imageFile != null)
            {
                var imageValidation = ValidateFile(imageFile, AllowedImageExtensions, "obrazu");
                if (imageValidation != null)
                {
                    ModelState.AddModelError("imageFile", imageValidation);
                }
            }

            // Walidacja pliku audio
            if (audioFile != null)
            {
                var audioValidation = ValidateFile(audioFile, AllowedAudioExtensions, "audio");
                if (audioValidation != null)
                {
                    ModelState.AddModelError("audioFile", audioValidation);
                }
            }

            // Usuñ walidacjê dla pól nawigacyjnych
            ModelState.Remove("ImageFileName");
            ModelState.Remove("AudioFileName");
            ModelState.Remove("Transitions");
            ModelState.Remove("Building");

            // SprawdŸ czy s¹ b³êdy walidacji PRZED prób¹ zapisu plików
            if (!ModelState.IsValid)
            {
                ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Name", location.BuildingId);
                return View(location);
            }

            // 1. Obs³uga zdjêcia
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName).ToLower();
                    string path = Path.Combine(wwwRootPath, "images", fileName);

                    Directory.CreateDirectory(Path.Combine(wwwRootPath, "images"));

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    location.ImageFileName = fileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("imageFile", $"B³¹d podczas zapisywania obrazu: {ex.Message}");
                    ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Name", location.BuildingId);
                    return View(location);
                }
            }
            else
            {
                // WA¯NE: Ustaw domyœlny obraz jeœli nie przes³ano pliku
                location.ImageFileName = "default.jpg";
            }

            // 2. Obs³uga audio
            if (audioFile != null && audioFile.Length > 0)
            {
                try
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(audioFile.FileName).ToLower();
                    string path = Path.Combine(wwwRootPath, "audio", fileName);

                    Directory.CreateDirectory(Path.Combine(wwwRootPath, "audio"));

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await audioFile.CopyToAsync(fileStream);
                    }
                    location.AudioFileName = fileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("audioFile", $"B³¹d podczas zapisywania audio: {ex.Message}");
                    ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Name", location.BuildingId);
                    return View(location);
                }
            }

            // Upewnij siê, ¿e ImageFileName nie jest null
            if (string.IsNullOrEmpty(location.ImageFileName))
            {
                location.ImageFileName = "default.jpg";
            }

            try
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                TempData["Message"] = $"Lokacja \"{location.Name}\" zosta³a utworzona.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"B³¹d podczas zapisywania do bazy: {ex.Message}");
                ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Name", location.BuildingId);
                return View(location);
            }
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
        [RequestSizeLimit(52428800)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,BuildingId,ImageAltText,IsHidden,Type,Floor")] Location location, IFormFile? imageFile, IFormFile? audioFile)
        {
            if (id != location.Id) return NotFound();

            var oldLocation = await _context.Locations
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);

            if (oldLocation == null) return NotFound();

            // Walidacja plików
            if (imageFile != null)
            {
                var imageValidation = ValidateFile(imageFile, AllowedImageExtensions, "obrazu");
                if (imageValidation != null)
                {
                    ModelState.AddModelError("imageFile", imageValidation);
                }
            }

            if (audioFile != null)
            {
                var audioValidation = ValidateFile(audioFile, AllowedAudioExtensions, "audio");
                if (audioValidation != null)
                {
                    ModelState.AddModelError("audioFile", audioValidation);
                }
            }

            ModelState.Remove("ImageFileName");
            ModelState.Remove("AudioFileName");
            ModelState.Remove("Transitions");
            ModelState.Remove("Building");

            if (!ModelState.IsValid)
            {
                ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Name", location.BuildingId);
                return View(location);
            }

            // 2. Obs³uga Zdjêcia
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName).ToLower();
                    string path = Path.Combine(wwwRootPath, "images", fileName);

                    Directory.CreateDirectory(Path.Combine(wwwRootPath, "images"));

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    location.ImageFileName = fileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("imageFile", $"B³¹d podczas zapisywania obrazu: {ex.Message}");
                    location.ImageFileName = oldLocation.ImageFileName;
                }
            }
            else
            {
                // Zachowaj star¹ nazwê pliku
                location.ImageFileName = oldLocation.ImageFileName;
            }

            // 3. Obs³uga Audio
            if (audioFile != null && audioFile.Length > 0)
            {
                try
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(audioFile.FileName).ToLower();
                    string path = Path.Combine(wwwRootPath, "audio", fileName);

                    Directory.CreateDirectory(Path.Combine(wwwRootPath, "audio"));

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await audioFile.CopyToAsync(stream);
                    }
                    location.AudioFileName = fileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("audioFile", $"B³¹d podczas zapisywania audio: {ex.Message}");
                    location.AudioFileName = oldLocation.AudioFileName;
                }
            }
            else
            {
                location.AudioFileName = oldLocation.AudioFileName;
            }

            // Upewnij siê, ¿e ImageFileName nie jest null
            if (string.IsNullOrEmpty(location.ImageFileName))
            {
                location.ImageFileName = oldLocation.ImageFileName ?? "default.jpg";
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = $"Lokacja \"{location.Name}\" zosta³a zaktualizowana.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Name", location.BuildingId);
            return View(location);
        }

        // POST: Locations/ToggleVisibility/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleVisibility(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null) return NotFound();

            location.IsHidden = !location.IsHidden;
            await _context.SaveChangesAsync();

            TempData["Message"] = location.IsHidden 
                ? $"Lokacja \"{location.Name}\" zosta³a ukryta." 
                : $"Lokacja \"{location.Name}\" jest teraz widoczna.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Locations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var location = await _context.Locations
                .Include(l => l.Building)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null) return NotFound();

            // SprawdŸ czy lokacja jest u¿ywana w przejœciach (jako Ÿród³o lub cel)
            var relatedTransitions = await _context.Transitions
                .Include(t => t.SourceLocation)
                .Include(t => t.TargetLocation)
                .Where(t => t.SourceLocationId == id || t.TargetLocationId == id)
                .ToListAsync();

            ViewBag.RelatedTransitions = relatedTransitions;
            ViewBag.HasRelatedTransitions = relatedTransitions.Any();

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
                // Najpierw usuñ wszystkie powi¹zane przejœcia (jako Ÿród³o lub cel)
                var relatedTransitions = await _context.Transitions
                    .Where(t => t.SourceLocationId == id || t.TargetLocationId == id)
                    .ToListAsync();

                if (relatedTransitions.Any())
                {
                    _context.Transitions.RemoveRange(relatedTransitions);
                }

                // Nastêpnie usuñ lokacjê
                _context.Locations.Remove(location);

                await _context.SaveChangesAsync();
                
                if (relatedTransitions.Any())
                {
                    TempData["Message"] = $"Lokacja \"{location.Name}\" oraz {relatedTransitions.Count} powi¹zanych przejœæ zosta³o usuniêtych.";
                }
                else
                {
                    TempData["Message"] = $"Lokacja \"{location.Name}\" zosta³a usuniêta.";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.Id == id);
        }

        /// <summary>
        /// Waliduje plik - sprawdza rozmiar i rozszerzenie
        /// </summary>
        private string? ValidateFile(IFormFile file, string[] allowedExtensions, string fileTypeName)
        {
            if (file.Length > MaxFileSize)
            {
                return $"Plik {fileTypeName} jest za du¿y ({file.Length / 1024 / 1024} MB). Maksymalny rozmiar to {MaxFileSize / 1024 / 1024} MB.";
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                return $"Niedozwolony format pliku {fileTypeName}. Dozwolone: {string.Join(", ", allowedExtensions)}";
            }

            return null;
        }
    }
}