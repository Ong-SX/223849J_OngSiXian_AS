using _223849J_OngSiXian.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _223849J_OngSiXian.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<Member> signInManager;
        private readonly IHttpContextAccessor contxt;
        public LogoutModel(SignInManager<Member> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            this.signInManager = signInManager;
            contxt = httpContextAccessor;
        }

        public void OnGet() { }
        
        public async Task<IActionResult> OnPostLogoutAsync()
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
            return RedirectToPage("Login");
        }
        public async Task<IActionResult> OnPostDontLogoutAsync()
        {
            return RedirectToPage("Index");
        }
    }
}
