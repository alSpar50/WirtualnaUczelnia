using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WirtualnaUczelnia.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Pole Email jest wymagane.")]
            [EmailAddress(ErrorMessage = "Niepoprawny format adresu email.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Pole Has³o jest wymagane.")]
            [DataType(DataType.Password)] // To mówi formularzowi, ¿e to pole has³a
            public string Password { get; set; }

            [Display(Name = "Zapamiêtaj mnie")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Wyczyœæ ciasteczka zewnêtrzne (np. Google/Facebook), aby zapewniæ czysty proces logowania
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // Próba logowania
                // lockoutOnFailure: false oznacza, ¿e konto nie zostanie zablokowane po nieudanych próbach (dla uproszczenia)
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("U¿ytkownik zalogowany.");
                    return LocalRedirect(returnUrl);
                }
                // Tutaj mo¿na obs³u¿yæ 2FA lub blokadê konta, ale dla uproszczenia pomijamy
                else
                {
                    ModelState.AddModelError(string.Empty, "Nieudana próba logowania. SprawdŸ email i has³o.");
                    return Page();
                }
            }

            // Jeœli dotarliœmy tutaj, coœ posz³o nie tak (np. b³¹d walidacji), wyœwietl formularz ponownie
            return Page();
        }
    }
}