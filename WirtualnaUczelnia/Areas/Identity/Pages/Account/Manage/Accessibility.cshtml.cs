using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WirtualnaUczelnia.Data;
using WirtualnaUczelnia.Models;

namespace WirtualnaUczelnia.Areas.Identity.Pages.Account.Manage
{
    public class AccessibilityModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AccessibilityModel(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public bool IsDisabled { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie mo¿na za³adowaæ u¿ytkownika o ID '{_userManager.GetUserId(User)}'.");
            }

            var pref = await _context.UserPreferences.FirstOrDefaultAsync(p => p.UserId == user.Id);
            
            Input = new InputModel
            {
                IsDisabled = pref?.IsDisabled ?? false
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie mo¿na za³adowaæ u¿ytkownika o ID '{_userManager.GetUserId(User)}'.");
            }

            var pref = await _context.UserPreferences.FirstOrDefaultAsync(p => p.UserId == user.Id);
            
            if (pref == null)
            {
                // Utwórz nowy rekord preferencji
                pref = new UserPreference
                {
                    UserId = user.Id,
                    IsDisabled = Input.IsDisabled
                };
                _context.UserPreferences.Add(pref);
            }
            else
            {
                // Zaktualizuj istniej¹cy
                pref.IsDisabled = Input.IsDisabled;
            }

            await _context.SaveChangesAsync();

            StatusMessage = Input.IsDisabled 
                ? "Ustawienia zapisane. Nawigacja bêdzie teraz omijaæ schody."
                : "Ustawienia zapisane. Nawigacja bêdzie u¿ywaæ wszystkich tras.";

            return RedirectToPage();
        }
    }
}
