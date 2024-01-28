using _223849J_OngSiXian.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _223849J_OngSiXian.Pages
{
	[Authorize]
	public class PrivacyModel : PageModel
	{
        /*private readonly ILogger<PrivacyModel> _logger;

		public PrivacyModel(ILogger<PrivacyModel> logger)
		{
			_logger = logger;
		}*/

        private readonly SignInManager<Member> signInManager;
        private readonly IHttpContextAccessor contxt;
        public PrivacyModel(SignInManager<Member> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            this.signInManager = signInManager;
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
    }
}