using _223849J_OngSiXian.Model;
using _223849J_OngSiXian.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace _223849J_OngSiXian.Pages
{
    public class ForgetPasswordModel : PageModel
    {
        [BindProperty]
        public forgetPW forgetPassword { get; set; }

        private readonly SignInManager<Member> signInManager;
        private UserManager<Member> userManager { get; }
        private readonly IHttpContextAccessor contxt;
        private readonly ILogger<ForgetPasswordModel> _logger;
        private readonly IEmailSender emailSender;

        public ForgetPasswordModel(SignInManager<Member> signInManager, UserManager<Member> userManager, IHttpContextAccessor httpContextAccessor, ILogger<ForgetPasswordModel> logger, IEmailSender emailSender)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            contxt = httpContextAccessor;
            _logger = logger;
            this.emailSender = emailSender;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(forgetPassword.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email");
                    return Page();
                }
                
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = "/ResetPassword?token=" + Uri.EscapeDataString(token) + "?email=" + user.Email;
                //await emailSender.SendEmailAsync(user.Email, "Reset Password Link", $"Please reset your password by clicking here: <a href='{callbackUrl}'>Reset Password</a>");

                var apiKey = "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";
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("keyon.cummings39@ethereal.email", "bookwormsonline");
                var subject = "Reset password link";
                var to = new EmailAddress("keyon.cummings39@ethereal.email", user.FirstName);
                var plainTextContent = "Please reset your password by clicking here:";
                var htmlContent = $"<a href='{callbackUrl}'>Reset Password</a>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                //var response = await client.SendEmailAsync(msg);
                await client.SendEmailAsync(msg);

                ModelState.AddModelError("", "Do check your email for the password reset link");
            }
            return Page();
        }
    }
}
