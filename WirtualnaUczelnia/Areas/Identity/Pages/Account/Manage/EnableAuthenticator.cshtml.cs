using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WirtualnaUczelnia.Areas.Identity.Pages.Account.Manage
{
    public class EnableAuthenticatorModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UrlEncoder _urlEncoder;

        public EnableAuthenticatorModel(UserManager<IdentityUser> userManager, UrlEncoder urlEncoder)
        {
            _userManager = userManager;
            _urlEncoder = urlEncoder;
        }

        public string SharedKey { get; set; }
        public string AuthenticatorUri { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(7, MinimumLength = 6, ErrorMessage = "Kod musi mieæ 6 cyfr.")]
            [DataType(DataType.Text)]
            [Display(Name = "Kod weryfikacyjny")]
            public string Code { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("B³¹d ³adowania u¿ytkownika.");

            await LoadSharedKeyAndQrCodeUriAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("B³¹d ³adowania u¿ytkownika.");

            if (!ModelState.IsValid)
            {
                await LoadSharedKeyAndQrCodeUriAsync(user);
                return Page();
            }

            // Weryfikacja kodu wpisanego przez u¿ytkownika
            var verificationCode = Input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);
            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                ModelState.AddModelError("Input.Code", "Kod jest nieprawid³owy.");
                await LoadSharedKeyAndQrCodeUriAsync(user);
                return Page();
            }

            // Sukces: W³¹czamy 2FA dla u¿ytkownika
            await _userManager.SetTwoFactorEnabledAsync(user, true);

            return RedirectToPage("./TwoFactorAuthentication"); // Mo¿esz tu przekierowaæ gdzie chcesz, np. do Index
        }

        private async Task LoadSharedKeyAndQrCodeUriAsync(IdentityUser user)
        {
            // Pobieramy lub generujemy klucz
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            SharedKey = unformattedKey;
            var email = await _userManager.GetEmailAsync(user);

            // Format URI dla Google Authenticator
            AuthenticatorUri = string.Format(
                "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6",
                _urlEncoder.Encode("WirtualnaUczelnia"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }
    }
}