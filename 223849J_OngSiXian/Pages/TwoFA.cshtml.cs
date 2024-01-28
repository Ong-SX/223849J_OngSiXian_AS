using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using _223849J_OngSiXian.Model;
using _223849J_OngSiXian.ViewModels;
using Microsoft.AspNetCore.DataProtection;

namespace _223849J_OngSiXian.Pages
{
    public class TwoFAModel : PageModel
    {
        [BindProperty]
        public TwoFA twoFA { get; set; }

        private readonly SignInManager<Member> signInManager;
        private UserManager<Member> userManager { get; }
        private readonly IHttpContextAccessor contxt;
        private readonly ILogger<TwoFAModel> _logger;
        private readonly IEmailSender emailSender;

        public TwoFAModel(SignInManager<Member> signInManager, UserManager<Member> userManager, IHttpContextAccessor httpContextAccessor, ILogger<TwoFAModel> logger, IEmailSender emailSender)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            contxt = httpContextAccessor;
            _logger = logger;
            this.emailSender = emailSender;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Ensure the user has login with login credentials first
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                //If user has not login with credientials yet redirect to login page
                return RedirectToPage("Login");
            }

            //Otherwise, send user the code via email
            var token = await userManager.GenerateTwoFactorTokenAsync(user, "Email");
            await emailSender.SendEmailAsync(user.Email, "2FA Verification Code",
                $"<h3 >{token}</h3>.");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

                if (user == null)
                {
                    //If user has not login with credientials yet redirect to login page
                    return RedirectToPage("Login");
                }

                //var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, Input.RememberMachine);
                //Verifies the 2FA code
                var verified = await signInManager.TwoFactorSignInAsync("Email", twoFA.Code, twoFA.RememberMe, false);
                //var userId = await userManager.GetUserIdAsync(user);

                if (verified.Succeeded)
                {
                    _logger.LogInformation("Logged in with 2fa.");
                    //Set user session
                    var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                    var protectCard = dataProtectionProvider.CreateProtector("MySecretKey");

                    contxt.HttpContext.Session.SetString("FirstName", user.FirstName);
                    contxt.HttpContext.Session.SetString("LastName", user.LastName);
                    contxt.HttpContext.Session.SetString("Email", user.Email);
                    contxt.HttpContext.Session.SetString("PhoneNumber", user.PhoneNumber);
                    contxt.HttpContext.Session.SetString("BillingAddress", user.BillingAddress);
                    contxt.HttpContext.Session.SetString("ShippingAddress", user.ShippingAddress);
                    contxt.HttpContext.Session.SetString("CreditCard", protectCard.Unprotect(user.CreditCard));
                    contxt.HttpContext.Session.SetString("TwoFA", user.TwoFactorEnabled.ToString());
                    return RedirectToPage("Index");
                }
                else if (verified.IsLockedOut)
                {
                    _logger.LogWarning("Account locked out.");
                    ModelState.AddModelError("", "Account is locked please try again later");
                }
                else
                {
                    _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
                    ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                    return Page();
                }
            }

            return Page();
        }
    }
}
