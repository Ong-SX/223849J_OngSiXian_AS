using _223849J_OngSiXian.Model;
using _223849J_OngSiXian.ViewModels;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace _223849J_OngSiXian.Pages
{
    public class changePasswordModel : PageModel
    {
        [BindProperty]
        public changePW changePassword { get; set; }

        private readonly SignInManager<Member> signInManager;
        private UserManager<Member> userManager { get; }
        private readonly IHttpContextAccessor contxt;
        private readonly ILogger<changePasswordModel> _logger;

        public changePasswordModel(SignInManager<Member> signInManager, UserManager<Member> userManager, IHttpContextAccessor httpContextAccessor, ILogger<changePasswordModel> logger)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            contxt = httpContextAccessor;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                var changedPassword = await userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);
                if (changedPassword.Succeeded)
                {
                    //Incorrect login credentials
                    ModelState.AddModelError("", "Password changed successfully");

                    return RedirectToPage("Index");
                }
                else
                {
                    //Incorrect login credentials
                    ModelState.AddModelError("", "Unable to change password");
                }

                foreach (var error in changedPassword.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }
    }
}
