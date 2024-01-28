using _223849J_OngSiXian.Model;
using _223849J_OngSiXian.ViewModels;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;


namespace _223849J_OngSiXian.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Login LMember { get; set; }

        private readonly SignInManager<Member> signInManager;
        private UserManager<Member> userManager { get; }
        private readonly IHttpContextAccessor contxt;
        private readonly ILogger<LoginModel> _logger;

        private readonly ICaptchaValidator _captchaValidator;
        public LoginModel(SignInManager<Member> signInManager, UserManager<Member> userManager, IHttpContextAccessor httpContextAccessor, ICaptchaValidator captchaValidator, ILogger<LoginModel> logger)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            contxt = httpContextAccessor;
            _captchaValidator = captchaValidator;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(string captcha)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(captcha))
            {
                ModelState.AddModelError("captcha", "Captcha validation failed");
            }
            if (ModelState.IsValid)
            {
                var loginOutcome = await signInManager.PasswordSignInAsync(HttpUtility.HtmlEncode(LMember.Email), HttpUtility.HtmlEncode(LMember.Password), LMember.RememberMe , true);
                if (loginOutcome.Succeeded)
                {
                    var userDetail = await userManager.FindByEmailAsync(LMember.Email);

                    //Decrypt credit card
                    var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                    var protectCard = dataProtectionProvider.CreateProtector("MySecretKey");

                    contxt.HttpContext.Session.SetString("FirstName", userDetail.FirstName);
                    contxt.HttpContext.Session.SetString("LastName", userDetail.LastName);
                    contxt.HttpContext.Session.SetString("Email", userDetail.Email);
                    contxt.HttpContext.Session.SetString("PhoneNumber", userDetail.PhoneNumber);
                    contxt.HttpContext.Session.SetString("BillingAddress", userDetail.BillingAddress);
                    contxt.HttpContext.Session.SetString("ShippingAddress", userDetail.ShippingAddress);
                    contxt.HttpContext.Session.SetString("CreditCard", protectCard.Unprotect(userDetail.CreditCard));
                    contxt.HttpContext.Session.SetString("TwoFA", userDetail.TwoFactorEnabled.ToString());

                    return RedirectToPage("Index");
                }
                if (loginOutcome.IsLockedOut)
                {
                    //Inform user to try again later since they have reached the max number of attempt
                    ModelState.AddModelError("", "Account is locked please try again later");
                }
                else
                {
                    //Incorrect login credentials
                    ModelState.AddModelError("", "Email or Password incorrect");
                }
            }
            return Page();
        }
    }
}
