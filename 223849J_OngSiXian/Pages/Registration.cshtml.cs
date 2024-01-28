using _223849J_OngSiXian.Model;
using _223849J_OngSiXian.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace _223849J_OngSiXian.Pages
{
    public class RegistrationModel : PageModel
    {
		private UserManager<Member> userManager { get; }

		[BindProperty]
		public Register RMember { get; set; }
		public RegistrationModel(UserManager<Member> userManager)
		{
			this.userManager = userManager;
		}

        public void OnGet()
        {
        }

        //Save data into the database
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				if (!RMember.Photo.EndsWith(".jpg"))
				{
                    ModelState.AddModelError(RMember.Photo, "You are only allowed to upload .jpg photos");
					return Page();
                }

				//Encrypt credit card
				var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
				var protectCard = dataProtectionProvider.CreateProtector("MySecretKey");

				var member = new Member()
				{
					UserName = HttpUtility.HtmlEncode(RMember.Email),
					Email = HttpUtility.HtmlEncode(RMember.Email),
					PhoneNumber = HttpUtility.HtmlEncode(RMember.PhoneNumber),
					FirstName = HttpUtility.HtmlEncode(RMember.FirstName),
					LastName = HttpUtility.HtmlEncode(RMember.LastName),
					CreditCard = protectCard.Protect(HttpUtility.HtmlEncode(RMember.CreditCard)),
					BillingAddress = HttpUtility.HtmlEncode(RMember.BillingAddress),
					ShippingAddress = HttpUtility.HtmlEncode(RMember.ShippingAddress),
					Photo = HttpUtility.HtmlEncode(RMember.Photo)
                };

				var result = await userManager.CreateAsync(member, RMember.Password);
				if (result.Succeeded)
				{
					//Redirect user to login page so that they can login with their registered details
					return RedirectToPage("Login");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return Page();
		}
    }
}
