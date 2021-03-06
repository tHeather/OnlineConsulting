using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OnlineConsulting.Constants;
using OnlineConsulting.Models.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<User> signInManager,
            ILogger<LoginModel> logger,
            UserManager<User> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);

            if (ModelState.IsValid && user != null)
            {

                if (user.IsAccountLocked) { 
                    ModelState.AddModelError(string.Empty, "Account is lock out.");
                    return Page();
                } 

                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                     _logger.LogInformation("User: {userId} logged in.", user.Id);

                    var roles = await _userManager.GetRolesAsync(user);
                    return roles[0] switch
                    {
                        UserRoleValue.ADMIN => RedirectToAction("list", "payment"),
                        UserRoleValue.CONSULTANT => RedirectToAction("NewConversationList", "Chat"),
                        UserRoleValue.EMPLOYER => RedirectToAction("GetStatistics", "Statistics"),
                        _ => throw new System.NotImplementedException(),
                    };
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            _logger.LogInformation("Invalid login attempt. User email: {email}", Input.Email);
            return Page();
        }
    }
}
