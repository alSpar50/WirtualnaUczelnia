using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WirtualnaUczelnia.Data;
using WirtualnaUczelnia.Tools;

namespace WirtualnaUczelnia.Controllers
{
    public class DataToolsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DataToolsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Export()
        {
            var exportPath = Path.Combine(_env.ContentRootPath, "DataExport");
            await DataMigrationTool.ExportDataAsync(_context, exportPath);
            TempData["Message"] = $"Dane wyeksportowane do: {exportPath}";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Import()
        {
            var importPath = Path.Combine(_env.ContentRootPath, "DataExport");
            await DataMigrationTool.ImportDataAsync(_context, importPath);
            TempData["Message"] = "Dane zaimportowane!";
            return RedirectToAction(nameof(Index));
        }
    }
}
