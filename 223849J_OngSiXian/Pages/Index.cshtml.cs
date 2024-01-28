using _223849J_OngSiXian.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _223849J_OngSiXian.Pages
{
    [Authorize]
    public class IndexModel : PageModel
	{
        private readonly ILogger<IndexModel> _logger;

        private readonly SignInManager<Member> signInManager;
        private UserManager<Member> userManager { get; }
        private readonly IHttpContextAccessor contxt;
        public IndexModel(UserManager<Member> userManager, SignInManager<Member> signInManager, IHttpContextAccessor httpContextAccessor, ILogger<IndexModel> logger)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            _logger = logger;
            contxt = httpContextAccessor;
        }


        public async void OnGet()
		{
            if (contxt.HttpContext.Session.GetString("Email") == null)
            {
                //Logout user
                await signInManager.SignOutAsync();
                //Clear session and redirect to login page
                contxt.HttpContext.Session.Clear();
                contxt.HttpContext.Session.Remove("FirstName");
                contxt.HttpContext.Session.Remove("LastName");
                contxt.HttpContext.Session.Remove("Email");
                contxt.HttpContext.Session.Remove("PhoneNumber");
                contxt.HttpContext.Session.Remove("BillingAddress");
                contxt.HttpContext.Session.Remove("ShippingAddress");
                contxt.HttpContext.Session.Remove("CreditCard");
                Response.Redirect("Login");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    //Enable 2FA
                    if (contxt.HttpContext.Session.GetString("TwoFA") == "False")
                    {
                        await userManager.SetTwoFactorEnabledAsync(user, true);
                        await signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation(1, "User enabled two-factor authentication.");
                        contxt.HttpContext.Session.SetString("TwoFA", user.TwoFactorEnabled.ToString());
                    }
                    //Disable 2FA
                    else
                    {
                        await userManager.SetTwoFactorEnabledAsync(user, false);
                        await signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation(2, "User disabled two-factor authentication.");
                        contxt.HttpContext.Session.SetString("TwoFA", user.TwoFactorEnabled.ToString());
                    }
                }
            }
            return Page();
        }

        private Task<Member> GetCurrentUserAsync()
        {
            //Get current login user
            return userManager.GetUserAsync(HttpContext.User);
        }
    }
}