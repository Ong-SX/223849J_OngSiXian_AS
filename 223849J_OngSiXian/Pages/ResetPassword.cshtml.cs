using _223849J_OngSiXian.Model;
using _223849J_OngSiXian.ViewModels;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Common;

namespace _223849J_OngSiXian.Pages
{
    public class ResetPasswordModel : PageModel
    {
        [BindProperty]
        public resetPW resetPassword { get; set; }

        private readonly SignInManager<Member> signInManager;
        private UserManager<Member> userManager { get; }
        private readonly IHttpContextAccessor contxt;
        private readonly ILogger<ResetPasswordModel> _logger;

        public ResetPasswordModel(SignInManager<Member> signInManager, UserManager<Member> userManager, IHttpContextAccessor httpContextAccessor, ILogger<ResetPasswordModel> logger)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            contxt = httpContextAccessor;
            _logger = logger;
        }

        public IActionResult OnGet(string token, string email)
        {
            resetPassword.Token = token;
            resetPassword.Email = email;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string token, string email)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(resetPassword.Email);
                if (user == null)
                {
                    return RedirectToPage("Login");
                }

                var resetPWResult = await userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);
                if (resetPWResult.Succeeded)
                {
                    return RedirectToPage("Login");
                }

                foreach (var error in resetPWResult.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            return Page();
        }
    }
}
