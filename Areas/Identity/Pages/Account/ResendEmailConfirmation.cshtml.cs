using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace OnlineConsulting.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ISendgridService _sendgridService;
        private readonly ILogger<ResendEmailConfirmationModel> _logger;

        public ResendEmailConfirmationModel(
            UserManager<User> userManager,
            ISendgridService sendgridService,
            ILogger<ResendEmailConfirmationModel> logger
            )
        {
            _userManager = userManager;
            _sendgridService = sendgridService;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                _logger.LogInformation(
                            "Failed resend verification email attempt. Not found user with email: {email}", 
                            Input.Email);

                StatusMessage = "Verification email sent. Please check your email.";
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);

            await _sendgridService.SendConfirmEmailAddressLink(
                Input.Email,
                callbackUrl);

            _logger.LogInformation("Verification email sent. Email: {email} ", Input.Email);

            StatusMessage = "Verification email sent. Please check your email.";
            return Page();
        }
    }
}
